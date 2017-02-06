using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public class InitChoixResto : DropCreateDatabaseAlways<BDDContext>
    {
        protected override void Seed(BDDContext context)
        {
            context.Restos.Add(new Resto { Id = 1, Name = "Resto pinambour", Phone = "123" });
            context.Restos.Add(new Resto { Id = 2, Name = "Resto pinière", Phone = "456" });
            context.Restos.Add(new Resto { Id = 3, Name = "Resto toro", Phone = "789" });

            base.Seed(context);
        }
    }
}