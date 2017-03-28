using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcore_default_null_bug.Contexts;
using Microsoft.EntityFrameworkCore;
using SqlOnlyMethodTests;

namespace efcore_default_null_bug
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = ConfigurationHelper.Configuration["ConnectionStrings.DefaultNullBug"];

            bool workingException = false; bool brokenException = false;

            using ( WorkingDbContext wctx = new WorkingDbContext( new DbContextOptionsBuilder<WorkingDbContext>().UseSqlServer( connectionString ).Options ) )
            {
                try
                {
                    wctx.Database.EnsureCreated();
                }
                catch ( Exception e )
                {
                    Console.WriteLine( "Failed to create working context:" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace );
                    workingException = true;
                }
                finally
                {
                    if ( !workingException )
                    {
                        Console.WriteLine( "Working context created successfully" );
                    }
                    wctx.Database.EnsureDeleted();
                }
            }

            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();

            using ( BrokenDbContext bctx = new BrokenDbContext( new DbContextOptionsBuilder<BrokenDbContext>().UseSqlServer( connectionString ).Options ) )
            {
                try
                {
                    bctx.Database.EnsureCreated();
                }
                catch ( Exception e )
                {
                    Console.WriteLine( "Failed to create broken context:" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace );
                    brokenException = true;
                }
                finally
                {
                    if ( !brokenException )
                    {
                        Console.WriteLine( "Broken context created successfully" );
                    }
                    bctx.Database.EnsureDeleted();
                }
            }
            Console.ReadLine();
        }
    }
}
