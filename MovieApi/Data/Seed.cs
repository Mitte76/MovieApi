using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieAPI.Data.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(MovieApiContext context)
        {
            var genreNames = new[] { "Crime", "Thriller", "Comedy", "Satire" };
            var genres = await context.Set<Genre>().ToListAsync();

            foreach (var genreName in genreNames)
            {
                if (!genres.Any(genre => string.Equals(
                    genre.GenreName,
                    genreName,
                    StringComparison.OrdinalIgnoreCase)))
                {
                    var genre = new Genre { GenreName = genreName };
                    context.Set<Genre>().Add(genre);
                    genres.Add(genre);
                }
            }

            await context.SaveChangesAsync();

            var genresByName = genres
                .GroupBy(genre => genre.GenreName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

            // Seed genre-katalogen även om filmer redan finns, men skapa inte filmer igen.
            if (await context.Movies.AnyAsync())
            {
                return;
            }

            var movies = new Movie[]
            {
                new Movie { Title = "De hänsynslösa (Reservoir Dogs)", Year = "1992", Duration = "99min" ,
                    Details = new MovieDetails {
                    Synopsis = "Efter ett misslyckat rån där polisen överraskar dem, flyr en grupp kriminella till en lagerlokal.",
                    Language = "Engelska",
                    Budget = "$1.2–3 million"
                    },
                    Actors = new Actor[]
                    {
                    new Actor
                    {
                        Name = "Harvey Keitel",
                        BirthYear = "1939"
                    },
                    new Actor
                    {
                        Name = "Tim Roth",
                        BirthYear = "1961"
                    }
                    },

                    Genres = [genresByName["Crime"], genresByName["Thriller"]],
                    Reviews = new Review[] {
                        new Review {
                            ReviewerName = "Adam Brannon",
                            Comment = "After last year’s bitterly disappointing Once Upon A Time in Hollywood bla bla.",
                            Rating = 4
                        }
                    }
                },
                new Movie { Title = "Monty Python's Life of Brian", Year = "1979 ", Duration = "94 min" ,
                    Details = new MovieDetails {
                        Synopsis = "The film's themes of religious satire were controversial at the time of its release, " +
                        "drawing accusations of blasphemy and protests from some religious groups..",
                        Language = "Engelska",
                        Budget = "$4 million"
                    },

                    Actors = new Actor[]
                    {
                    new Actor
                    {
                        Name = "Graham Chapman",
                        BirthYear = "1941"
                    },
                    new Actor
                    {
                        Name = "John Cleese",
                        BirthYear = "1939"
                    },
                    new Actor
                    {
                        Name = "Eric Idle",
                        BirthYear = "1943"
                    }
                    },

                    Genres = [genresByName["Comedy"], genresByName["Satire"]],
                    Reviews = new Review[] {
                        new Review {
                            ReviewerName = "Adam Brannon",
                            Comment = "After last year’s bitterly disappointing Once Upon A Time in Hollywood bla bla.",
                            Rating = 4
                        }
                    }
                },
                new Movie { Title = "Django Unchained", Year = "2012", Duration = "165min" ,
                   Details = new MovieDetails {
                        Synopsis = "The film is a highly stylized, revisionist tribute to spaghetti Westerns, with its title referring " +
                        "particularly to the 1966 Italian film Django by Sergio Corbucci. Development of Django Unchained " +
                        "began in 2007, when Tarantino was writing a book on Corbucci. By April 2011, Tarantino sent his " +
                        "final draft of the script to the Weinstein Company (TWC).",
                        Language = "Engelska",
                        Budget = "$4 million"
                    },
                    Actors = new Actor[]
                    {
                      new Actor
                      {
                        Name = "Jamie Foxx",
                        BirthYear = "1967"
                      },
                      new Actor
                      {
                        Name = "Christoph Waltz",
                        BirthYear = "1956"
                      }
                    },

                    Genres = [genresByName["Crime"], genresByName["Thriller"]],
                    Reviews = new Review[] {
                        new Review {
                            ReviewerName = "Adam Brannon",
                            Comment = "After last year’s bitterly disappointing Once Upon A Time in Hollywood bla bla.",
                            Rating = 4
                        }
                    }
                },
            };

            context.Movies.AddRange(movies);
            await context.SaveChangesAsync();
        }
    }
}
