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

            var actorBirthYears = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Harvey Keitel"] = "1939",
                ["Tim Roth"] = "1961",
                ["Graham Chapman"] = "1941",
                ["John Cleese"] = "1939",
                ["Eric Idle"] = "1943",
                ["Jamie Foxx"] = "1967",
                ["Christoph Waltz"] = "1956"
            };

            var actors = await context.Set<Actor>().ToListAsync();
            foreach (var actorEntry in actorBirthYears)
            {
                if (!actors.Any(actor => string.Equals(
                    actor.Name,
                    actorEntry.Key,
                    StringComparison.OrdinalIgnoreCase)))
                {
                    var actor = new Actor
                    {
                        Name = actorEntry.Key,
                        BirthYear = actorEntry.Value
                    };
                    context.Set<Actor>().Add(actor);
                    actors.Add(actor);
                }
            }

            await context.SaveChangesAsync();

            var actorsByName = actors
                .GroupBy(actor => actor.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

            var seededMovieActors = new Dictionary<string, string[]>
            {
                ["De hänsynslösa (Reservoir Dogs)"] = ["Harvey Keitel", "Tim Roth"],
                ["Monty Python's Life of Brian"] = ["Graham Chapman", "John Cleese", "Eric Idle"],
                ["Django Unchained"] = ["Jamie Foxx", "Christoph Waltz"]
            };

            var seededMovieGenres = new Dictionary<string, string[]>
            {
                ["De hänsynslösa (Reservoir Dogs)"] = ["Crime", "Thriller"],
                ["Monty Python's Life of Brian"] = ["Comedy", "Satire"],
                ["Django Unchained"] = ["Crime", "Thriller"]
            };

            var existingSeededMovies = await context.Movies
                .Include(movie => movie.Genres)
                .Include(movie => movie.Actors)
                .Where(movie => seededMovieGenres.Keys.Contains(movie.Title))
                .ToListAsync();

            foreach (var movie in existingSeededMovies)
            {
                movie.Genres.Clear();
                foreach (var genreName in seededMovieGenres[movie.Title])
                {
                    movie.Genres.Add(genresByName[genreName]);
                }

                movie.Actors.Clear();
                foreach (var actorName in seededMovieActors[movie.Title])
                {
                    movie.Actors.Add(actorsByName[actorName]);
                }
            }

            if (existingSeededMovies.Count > 0)
            {
                await context.SaveChangesAsync();
            }

            // Seed genre-katalogen even when movies already exist, but don't add sample movies twice.
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
                    Actors = [actorsByName["Harvey Keitel"], actorsByName["Tim Roth"]],

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

                    Actors = [
                        actorsByName["Graham Chapman"],
                        actorsByName["John Cleese"],
                        actorsByName["Eric Idle"]
                    ],

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
                    Actors = [actorsByName["Jamie Foxx"], actorsByName["Christoph Waltz"]],

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
