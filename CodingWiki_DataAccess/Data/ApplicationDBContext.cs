using CodingWiki_DataAccess.FluentConfig;
using CodingWiki_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace CodingWiki_DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookDetail> BookDetails { get; set; }
        public DbSet<Fluent_Book> Fluent_Books { get; set; }
        public DbSet<Fluent_Author> Fluent_Authors { get; set; }
        public DbSet<Fluent_Publisher> Fluent_Publishers { get; set; }
        public DbSet<BookAuthorMap> BookAuthorMaps { get; set; }
        public DbSet<Fluent_BookAuthorMap> Fluent_BookAuthorMaps { get; set; }
        public DbSet<Fluent_BookDetail> BookDetail_fluent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Server=SKYNET;Database=CodingWiki;TrustServerCertificate=True;Trusted_Connection=True;")
                    .LogTo(LogDbCommands, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(u => u.Price).HasPrecision(10, 5);

            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Spider without Duty", ISBN = "123B12", Price = 10.99m, Publisher_Id = 1 },
                new Book { BookId = 2, Title = "Fortune of Time", ISBN = "12123B12", Price = 11.99m, Publisher_Id = 1 },
                new Book { BookId = 3, Title = "Fake Sunday", ISBN = "77652", Price = 20.99m, Publisher_Id = 2 },
                new Book { BookId = 4, Title = "Cookie Jar", ISBN = "CC12B12", Price = 25.99m, Publisher_Id = 3 },
                new Book { BookId = 5, Title = "Cloudy Forest", ISBN = "90392B33", Price = 40.99m, Publisher_Id = 3 }
            );

            modelBuilder.Entity<BookAuthorMap>().HasKey(u => new { u.Author_Id, u.Book_Id });

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Publisher_Id = 1, Name = "Pub 1 Jimmy", Location = "Chicago" },
                new Publisher { Publisher_Id = 2, Name = "Pub 2 John", Location = "New York" },
                new Publisher { Publisher_Id = 3, Name = "Pub 3 Ben", Location = "Hawaii" }
            );

            modelBuilder.ApplyConfiguration(new FluentAuthorConfig());
            modelBuilder.ApplyConfiguration(new FluentBookAuthorMapConfig());
            modelBuilder.ApplyConfiguration(new FluentBookConfig());
            modelBuilder.ApplyConfiguration(new FluentBookDetailConfig());
            modelBuilder.ApplyConfiguration(new FluentPublisherConfig());
        }



        private void LogDbCommands(string log)
        {
            if (log.Contains("Executed DbCommand"))
            {
                var timestamp = log.Substring(6, 23);
                log = log.Replace($"info: {timestamp} ", "");

                // Escape potenziell problematische Zeichen
                string safeLog = Markup.Escape(log);

                // Execution Time & Parameter-Block extrahieren
                var commandMatch = Regex.Match(log, @"Executed DbCommand \((\d+)ms\) \[(.*?)\]", RegexOptions.Singleline);
                string executionTime = commandMatch.Success ? $"{commandMatch.Groups[1].Value}ms" : "N/A";
                string parametersRaw = commandMatch.Success ? commandMatch.Groups[2].Value : "Keine Parameter";

                // SQL-Statement extrahieren (alles nach der 2. Zeile)
                string[] logLines = log.Split('\n');
                var sqlStatement = string.Join("\n", logLines.Skip(2)).Trim();

                // Strukturierte Parameter-Tabelle
                var paramTable = new Table().RoundedBorder();
                paramTable.AddColumn("[bold yellow]Parameter[/]");
                paramTable.AddColumn("[bold yellow]Type[/]");
                paramTable.AddColumn("[bold yellow]Value[/]");
                paramTable.AddColumn("[bold yellow]Size[/]");
                paramTable.AddColumn("[bold yellow]Precision[/]");
                paramTable.AddColumn("[bold yellow]Scale[/]");

                // Parameter einzeln extrahieren
                var paramMatches = Regex.Matches(parametersRaw, @"(@\w+)='([^']*)' \((.*?)\)");
                foreach (Match match in paramMatches)
                {
                    string paramName = match.Groups[1].Value;
                    string paramValue = match.Groups[2].Value == "?" ? "[gray](NULL)[/]" : match.Groups[2].Value;
                    string[] paramDetails = match.Groups[3].Value.Split(new[] { ',' }, StringSplitOptions.TrimEntries);

                    string type = paramDetails.Length > 0 ? paramDetails[0] : "Unknown";
                    string size = paramDetails.Length > 1 ? paramDetails[1] : "-";
                    string precision = paramDetails.Length > 2 ? paramDetails[2] : "-";
                    string scale = paramDetails.Length > 3 ? paramDetails[3] : "-";

                    paramTable.AddRow(paramName, $"[blue]{type}[/]", paramValue, size, precision, scale);
                }

                // Ausgabe formatieren
                AnsiConsole.MarkupLine($"\n[gray]{timestamp}[/] [bold cyan]SQL Execution:[/]");

                var detailsTable = new Table().RoundedBorder();
                detailsTable.AddColumn("[bold yellow]Details[/]");
                detailsTable.AddColumn("[bold yellow]Werte[/]");
                detailsTable.AddRow("[bold]Execution Time:[/]", $"[green]{executionTime}[/]");

                AnsiConsole.Write(detailsTable);

                // Parameter-Tabelle nur ausgeben, wenn Parameter vorhanden sind
                if (paramMatches.Count > 0)
                {
                    AnsiConsole.MarkupLine("\n[bold cyan]SQL Parameters:[/]");
                    AnsiConsole.Write(paramTable);
                }

                // SQL-Statement als Block anzeigen
                AnsiConsole.Write(new Panel($"[yellow]{Markup.Escape(sqlStatement)}[/]")
                    .Header("SQL Query", Justify.Center)
                    .RoundedBorder()
                    .Expand());
            }
            else
            {
                Console.WriteLine(log);
            }
        }
    }
}
