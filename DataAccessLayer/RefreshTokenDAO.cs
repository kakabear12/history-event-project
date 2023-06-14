using BusinessObjectsLayer.Models;
using DTOs.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RefreshTokenDAO
    {
        private readonly HistoryEventDBContext context;
        public RefreshTokenDAO(HistoryEventDBContext context)
        {
            this.context = context;
        }
        public async Task<RefreshToken> GetRefreshTokenByUserIdAsync(int userID)
        {
            try
            {
                return await context.RefreshTokens.FirstOrDefaultAsync(o => o.UserId == userID);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<int> RemoveRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                context.RefreshTokens.Remove(refreshToken);

                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
