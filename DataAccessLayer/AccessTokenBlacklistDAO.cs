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
    public class AccessTokenBlacklistDAO
    {
        private readonly HistoryEventDBContext context;
        public AccessTokenBlacklistDAO(HistoryEventDBContext context)
        {
            this.context = context;
        }
        public async Task<List<AccessTokenBlacklist>> GetBlacklistsAsync()
        {
            try
            {
                return await context.AccessTokenBlacklists.ToListAsync();
            }catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public void AddBlacklist(AccessTokenBlacklist blacklist)
        {
            try
            {
                context.AccessTokenBlacklists.Add(blacklist);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

    }
}
