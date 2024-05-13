using Darts_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Repository
{
    public class PlayerRepository : Repository<Player>, IRepository<Player>
    {
        public PlayerRepository(DartsDbContext ctx) : base(ctx)
        {
        }

        public override Player Read(int id)
        {
            return ctx.Players.FirstOrDefault(x => x.Id == id);
        }

        public override void Update(Player item)
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
