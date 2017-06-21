using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Web.Data
{
    public class InventoryContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                var context = (InventoryContext)applicationBuilder
                    .ApplicationServices.GetService(typeof(InventoryContext));

                context.Database.Migrate();

                if (!context.ComponentTypes.Any())
                {
                    context.ComponentTypes.AddRange(
                        GetPreconfiguredComponentTypes());

                    await context.SaveChangesAsync();
                }

                if (!context.Components.Any())
                {
                    context.Components.AddRange(
                        GetPreconfiguredComponents());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger("catalog seed");
                    log.LogError(ex.Message);
                    await SeedAsync(applicationBuilder, loggerFactory, retryForAvailability);
                }
            }
        }

        static IEnumerable<ComponentType> GetPreconfiguredComponentTypes()
        {
            return new List<ComponentType>()
            {
               new ComponentType() { Type="Microcontroller"},
               new ComponentType() { Type="System on Chip" },
               new ComponentType() { Type="Basic Computer" },
               new ComponentType() { Type="Digital Signal Processor" },
               new ComponentType() { Type="Other" }
            };
        }

        static IEnumerable<Component> GetPreconfiguredComponents()
        {
            return new List<Component>()
             {
                 new Component() { Type=1, Name="Arduino Uno", Manufacturer="Sunflower", Quantity=10, UnitPrice = 22.5M },
                 new Component() { Type=1, Name="Arduino Mega", Manufacturer="Sunflower", Quantity=10, UnitPrice= 35.50M },
                 new Component() { Type=1, Name="Arduino Nano", Manufacturer="Sunflower", Quantity=10, UnitPrice = 12.25M },
                 new Component() { Type=1, Name="Arduino Dou", Manufacturer="Sunflower", Quantity=10, UnitPrice = 30.75M },
                 new Component() { Type=2, Name="Edison", Manufacturer="Intel", Quantity=10, UnitPrice = 101.5M },
                 new Component() { Type=2, Name="ARM Cortex-M", Manufacturer="Cypress Semiconductor", Quantity=10, UnitPrice = 89.55M },
                 new Component() { Type=3, Name="Raspberry Pi 3 Model B", Manufacturer="Raspberry Pi Foundation",  Quantity=10, UnitPrice = 35.8M  },
                 new Component() { Type=3, Name="Raspberry Pi Zero W", Manufacturer="Raspberry Pi Foundation", Quantity=10, UnitPrice = 27.4M },
                 new Component() { Type=3, Name="Raspberry Pi 2 Model B", Manufacturer="Raspberry Pi Foundation", Quantity=10, UnitPrice = 30.75M },
                 new Component() { Type=3, Name="Raspberry Pi Zero", Manufacturer="Raspberry Pi Foundation", Quantity=10, UnitPrice = 25.45M },
                 new Component() { Type=4, Name="DSP 66AK2x", Manufacturer="Texas Insturments", Quantity=10, UnitPrice = 57.8M },
                 new Component() { Type=4, Name="DSP OMAP-L1x", Manufacturer="", Quantity=10, UnitPrice = 44.25M }
             };
        }
    }
}