using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet_tool.Data;
using Wallet_tool.Model.Domain;
using Wallet_tool.Model.DTO;

namespace Wallet_tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletDbContext walletDbContext;

        public WalletController(WalletDbContext walletDbContext)
        {
            this.walletDbContext = walletDbContext;
        }

        [HttpGet("account")]
        [Authorize(Roles ="admin, manager")]
        public async Task<IActionResult> GetWalletAccounts()
        {
            var walletAccounts = await walletDbContext.WalletAccount.ToListAsync();
            return Ok(walletAccounts);
        }

        
        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateWalletAccount([FromBody] AddWalletAccountDto addWalletAccountDto)
        {
            var walletAccount = new WalletAccount
            {
                AccountName = addWalletAccountDto.AccountName,
                ProjectName = addWalletAccountDto.ProjectName,
                ProjectManager = addWalletAccountDto.ProjectManager,
                Role = "manager"
            };

            await walletDbContext.WalletAccount.AddAsync(walletAccount);
            await walletDbContext.SaveChangesAsync();

            return Ok(walletAccount);
        }
        
        [HttpPut("update:{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateWalletAccount(Guid id, [FromBody] UpdateWalletAccountDto updateWalletAccountDto)
        {
            var walletAccount = await walletDbContext.WalletAccount.FirstOrDefaultAsync(x => x.Id == id);
            if (walletAccount == null)
            {
                return NotFound();
            }

            walletAccount.AccountName = updateWalletAccountDto.AccountName ?? walletAccount.AccountName;
            walletAccount.ProjectName = updateWalletAccountDto.ProjectName ?? walletAccount.ProjectName;
            walletAccount.ProjectManager = updateWalletAccountDto.ProjectManager ?? walletAccount.ProjectManager;

            await walletDbContext.SaveChangesAsync();

            return Ok(walletAccount);
        }
        
        [HttpDelete("delete:{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteWalletAccount(Guid id)
        {
            var walletAccount = await walletDbContext.WalletAccount.FirstOrDefaultAsync(x => x.Id == id);
            if (walletAccount == null)
            {
                return NotFound();
            }
            if (walletAccount.Balance != 0)
            {
                return BadRequest("Account must have 0 balance to delete");
            }

            walletDbContext.WalletAccount.Remove(walletAccount);
            await walletDbContext.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPost("deposit")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto depositDto)
        {
            var walletAccount = await walletDbContext.WalletAccount.FindAsync(depositDto.AccountId);
            if (walletAccount == null)
            {
                return NotFound("Account not found.");
            }

            walletAccount.Balance += depositDto.Amount;

            walletAccount.Status = walletAccount.Balance > 0 ? "Active" : "Inactive";

            var transaction = new Transaction
            {
                WalletAccountId = walletAccount.Id,
                To = depositDto.Account,
                Transaction_Type = "Deposit",
                Amount = depositDto.Amount,
                Remark = depositDto.Remark,
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            };

            await walletDbContext.Transactions.AddAsync(transaction);
            await walletDbContext.SaveChangesAsync();

            return Ok(transaction);
        }
        
        [HttpPost("withdraw")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawDto withdrawDto)
        {
            var walletAccount = await walletDbContext.WalletAccount.FindAsync(withdrawDto.AccountId);
            if (walletAccount == null)
            {
                return NotFound("Account not found.");
            }

            if (walletAccount.Balance < withdrawDto.Amount)
            {
                return BadRequest("Insufficient balance.");
            }

     
            walletAccount.Balance -= withdrawDto.Amount;
            walletAccount.Expenditure += withdrawDto.Amount;
            walletAccount.Status = walletAccount.Balance > 0 ? "Active" : "Inactive";

            var transaction = new Transaction
            {
                WalletAccountId = walletAccount.Id,
                From = withdrawDto.Account,
                Transaction_Type = "Withdraw",
                Amount = withdrawDto.Amount,
                Remark = withdrawDto.Remark,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            walletDbContext.Transactions.Add(transaction);
            await walletDbContext.SaveChangesAsync();

            return Ok(transaction);
        }
        
        [HttpPost("request")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> RequestWithdrawal([FromBody] RequestDto requestDto)
        {
            var walletAccount = await walletDbContext.WalletAccount.FindAsync(requestDto.FromId);
            if (walletAccount == null)
            {
                return NotFound("Account not found.");
            }

            if (walletAccount.Balance < requestDto.Amount)
            {
                return BadRequest("Insufficient balance.");
            }


            var transaction = new Transaction
            {
                WalletAccountId = walletAccount.Id,
                ToId = requestDto.ToId,
                From = requestDto.From,
                To = requestDto.To,
                Transaction_Type = "Request",
                Amount = requestDto.Amount,
                Remark = requestDto.Remark,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            walletDbContext.Transactions.Add(transaction);
            await walletDbContext.SaveChangesAsync();

            return Ok(transaction);
        }

        
        [HttpGet("getAllRequests")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllRequests()
        {
            var pendingRequests = await walletDbContext.Transactions
                .Where(t => t.Transaction_Type == "Request" && t.Status == "Pending")
                .ToListAsync();

            return Ok(pendingRequests);
        }

        [HttpPut("approve")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveRequest([FromQuery] Guid id)
        {
            var transaction = await walletDbContext.Transactions.FindAsync(id);
            if (transaction == null || transaction.Transaction_Type != "Request")
            {
                return NotFound("Request not found.");
            }
            if (transaction.Status != "Pending")
            {
                return BadRequest("Request is already processed.");
            }

            var fromAccount = await walletDbContext.WalletAccount.FindAsync(transaction.WalletAccountId);
            if (fromAccount == null)
            {
                return NotFound("Source account not found.");
            }

            if (fromAccount.Balance < transaction.Amount)
            {
                return BadRequest("Insufficient balance to approve the request.");
            }

            var toAccount = await walletDbContext.WalletAccount.FindAsync(transaction.ToId);
            if (toAccount == null)
            {
                return NotFound("Destination account not found.");
            }

            fromAccount.Balance -= transaction.Amount;
            fromAccount.Expenditure += transaction.Amount;
            toAccount.Balance += transaction.Amount;
            toAccount.Status = toAccount.Balance > 0 ? "Active" : "Inactive";
            fromAccount.Status = fromAccount.Balance > 0 ? "Active" : "Inactive";

            transaction.Status = "Approved";

            await walletDbContext.SaveChangesAsync();
            return Ok(transaction);
        }

        [HttpPut("reject")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectRequest([FromQuery] Guid id)
        {
            var transaction = await walletDbContext.Transactions.FindAsync(id);
            if (transaction == null || transaction.Transaction_Type != "Request")
            {
                return NotFound("Request not found.");
            }
            if (transaction.Status != "Pending")
            {
                return BadRequest("Request is already processed.");
            }

            transaction.Status = "Rejected";
            await walletDbContext.SaveChangesAsync();

            return Ok(transaction);
        }
        
        [HttpGet("transactions")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await walletDbContext.Transactions
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("balance")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> BalanceCheck([FromQuery] Guid accountId)
        {
            var walletAccount = await walletDbContext.WalletAccount.FindAsync(accountId);
            if (walletAccount == null)
            {
                return NotFound("Account not found.");
            }

            return Ok(new { balance = walletAccount.Balance});
        }

        [HttpPost("pay")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Pay([FromBody] RequestDto requestDto)
        { 
            var fromAccount = await walletDbContext.WalletAccount.FindAsync(requestDto.FromId);
            if (fromAccount == null)
            {
                return NotFound("Source account not found.");
            }

            if (fromAccount.Balance < requestDto.Amount)
            {
                return BadRequest("Insufficient balance to approve the request.");
            }

            var toAccount = await walletDbContext.WalletAccount.FindAsync(requestDto.ToId);
            if (toAccount == null)
            {
                return NotFound("Destination account not found.");
            }
            if (requestDto.FromId == requestDto.ToId)
            {
                return BadRequest("Source and destination accounts cannot be the same.");
            }

            fromAccount.Balance -= requestDto.Amount;
            fromAccount.Expenditure += requestDto.Amount;
            toAccount.Balance += requestDto.Amount;
            fromAccount.Status = fromAccount.Balance > 0 ? "Active" : "Inactive";
            toAccount.Status = toAccount.Balance > 0 ? "Active" : "Inactive";


            var transaction = new Transaction
            {
                WalletAccountId = fromAccount.Id,
                From = requestDto.From,
                To = requestDto.To,
                Transaction_Type = "Internal Payment",
                Amount = requestDto.Amount,
                Remark = requestDto.Remark,
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            };

            walletDbContext.Transactions.Add(transaction);
            await walletDbContext.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}
