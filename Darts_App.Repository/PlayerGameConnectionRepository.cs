using Darts_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Repository
{
    public class PlayerGameConnectionRepository : Repository<PlayerGameConnection>, IRepository<PlayerGameConnection>
    {
        public PlayerGameConnectionRepository(DartsDbContext ctx) : base(ctx)
        {
        }

        public override PlayerGameConnection Read(int id)
        {
            return ctx.PlayerGameConnection.FirstOrDefault(x => x.Id == id);
        }

        public override void Update(PlayerGameConnection item)
        {
            var oldItem = Read(item.Id);
            foreach (var prop in oldItem.GetType().GetProperties())
            {
                //prop.SetValue(oldItem, prop.GetValue(item));

                if (prop.GetAccessors().FirstOrDefault(t => t.IsVirtual) == null)
                {
                    prop.SetValue(oldItem, prop.GetValue(item));
                }
            }
            ctx.SaveChanges();
        }
    }
}
