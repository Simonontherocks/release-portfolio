using CineVault.BusinessLogic.Models;
using CineVault.DataAccessLayer;
using System.IO;

namespace CineVault.MainProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Mock data for Actor
            Actor actor1 = new Actor { Id = 1, Name = "Leonardo DiCaprio", IMDBEntry = new IMDBEntry { Name = "Leonardo DiCaprio", Url = "https://www.imdb.com/name/nm0000138/", Type = IMDBType.Actor } };
            Actor actor2 = new Actor { Id = 2, Name = "Morgan Freeman", IMDBEntry = new IMDBEntry { Name = "Morgan Freeman", Url = "https://www.imdb.com/name/nm0000151/", Type = IMDBType.Actor } };
            Actor actor3 = new Actor { Id = 3, Name = "Tom Hanks", IMDBEntry = new IMDBEntry { Name = "Tom Hanks", Url = "https://www.imdb.com/name/nm0000158/", Type = IMDBType.Actor } };
            Actor actor4 = new Actor { Id = 4, Name = "Meryl Streep", IMDBEntry = new IMDBEntry { Name = "Meryl Streep", Url = "https://www.imdb.com/name/nm0000658/", Type = IMDBType.Actor } };
            Actor actor5 = new Actor { Id = 5, Name = "Brad Pitt", IMDBEntry = new IMDBEntry { Name = "Brad Pitt", Url = "https://www.imdb.com/name/nm0000093/", Type = IMDBType.Actor } };
            Actor actor6 = new Actor { Id = 6, Name = "Scarlett Johansson", IMDBEntry = new IMDBEntry { Name = "Scarlett Johansson", Url = "https://www.imdb.com/name/nm0424060/", Type = IMDBType.Actor } };
            Actor actor7 = new Actor { Id = 7, Name = "Robert De Niro", IMDBEntry = new IMDBEntry { Name = "Robert De Niro", Url = "https://www.imdb.com/name/nm0000134/", Type = IMDBType.Actor } };
            Actor actor8 = new Actor { Id = 8, Name = "Jennifer Lawrence", IMDBEntry = new IMDBEntry { Name = "Jennifer Lawrence", Url = "https://www.imdb.com/name/nm2225369/", Type = IMDBType.Actor } };
            Actor actor9 = new Actor { Id = 9, Name = "Will Smith", IMDBEntry = new IMDBEntry { Name = "Will Smith", Url = "https://www.imdb.com/name/nm0000226/", Type = IMDBType.Actor } };
            Actor actor10 = new Actor { Id = 10, Name = "Angelina Jolie", IMDBEntry = new IMDBEntry { Name = "Angelina Jolie", Url = "https://www.imdb.com/name/nm0001401/", Type = IMDBType.Actor } };
            Actor actor11 = new Actor { Id = 11, Name = "Johnny Depp", IMDBEntry = new IMDBEntry { Name = "Johnny Depp", Url = "https://www.imdb.com/name/nm0000136/", Type = IMDBType.Actor } };
            Actor actor12 = new Actor { Id = 12, Name = "Emma Watson", IMDBEntry = new IMDBEntry { Name = "Emma Watson", Url = "https://www.imdb.com/name/nm0914612/", Type = IMDBType.Actor } };

            // Mock data for Director
            Director director1 = new Director { Id = 13, Name = "Christopher Nolan", IMDBEntry = new IMDBEntry { Name = "Christopher Nolan", Url = "https://www.imdb.com/name/nm0634240/", Type = IMDBType.Director } };
            Director director2 = new Director { Id = 14, Name = "Steven Spielberg", IMDBEntry = new IMDBEntry { Name = "Steven Spielberg", Url = "https://www.imdb.com/name/nm0000229/", Type = IMDBType.Director } };
            Director director3 = new Director { Id = 15, Name = "Quentin Tarantino", IMDBEntry = new IMDBEntry { Name = "Quentin Tarantino", Url = "https://www.imdb.com/name/nm0000233/", Type = IMDBType.Director } };
            Director director4 = new Director { Id = 16, Name = "Martin Scorsese", IMDBEntry = new IMDBEntry { Name = "Martin Scorsese", Url = "https://www.imdb.com/name/nm0000217/", Type = IMDBType.Director } };
            Director director5 = new Director { Id = 17, Name = "Ridley Scott", IMDBEntry = new IMDBEntry { Name = "Ridley Scott", Url = "https://www.imdb.com/name/nm0000631/", Type = IMDBType.Director } };

            // Mock data for Movie
            Movie movie1 = new Movie { Id = 18, Title = "Inception", Barcode = "1234567890123", Director = director1, Actors = new List<Actor> { actor1 }, IMDBEntry = new IMDBEntry { Name = "Inception", Url = "https://www.imdb.com/title/tt1375666/", Type = IMDBType.Movie }, Seen = true, Score = 9, Year = "2010" };
            Movie movie2 = new Movie { Id = 19, Title = "The Shawshank Redemption", Barcode = "9876543210987", Director = director2, Actors = new List<Actor> { actor2 }, IMDBEntry = new IMDBEntry { Name = "The Shawshank Redemption", Url = "https://www.imdb.com/title/tt0111161/", Type = IMDBType.Movie }, Seen = false, Score = 10, Year = "1994" };
            Movie movie3 = new Movie { Id = 20, Title = "Pulp Fiction", Barcode = "1234567890456", Director = director3, Actors = new List<Actor> { actor3, actor11 }, IMDBEntry = new IMDBEntry { Name = "Pulp Fiction", Url = "https://www.imdb.com/title/tt0110912/", Type = IMDBType.Movie }, Seen = true, Score = 8, Year = "1994" };
            Movie movie4 = new Movie { Id = 21, Title = "The Godfather", Barcode = "9876543210678", Director = director4, Actors = new List<Actor> { actor7 }, IMDBEntry = new IMDBEntry { Name = "The Godfather", Url = "https://www.imdb.com/title/tt0068646/", Type = IMDBType.Movie }, Seen = true, Score = 10, Year = "1972" };
            Movie movie5 = new Movie { Id = 22, Title = "Gladiator", Barcode = "1234509876123", Director = director5, Actors = new List<Actor> { actor5 }, IMDBEntry = new IMDBEntry { Name = "Gladiator", Url = "https://www.imdb.com/title/tt0172495/", Type = IMDBType.Movie }, Seen = true, Score = 9, Year = "2000" };
            Movie movie6 = new Movie { Id = 23, Title = "Titanic", Barcode = "3210987654321", Director = director2, Actors = new List<Actor> { actor1, actor10 }, IMDBEntry = new IMDBEntry { Name = "Titanic", Url = "https://www.imdb.com/title/tt0120338/", Type = IMDBType.Movie }, Seen = true, Score = 10, Year = "1997" };
            Movie movie7 = new Movie { Id = 24, Title = "The Matrix", Barcode = "5678901234567", Director = new Director { Id = 6, Name = "Lana Wachowski", IMDBEntry = new IMDBEntry { Name = "Lana Wachowski", Url = "https://www.imdb.com/name/nm0905154/", Type = IMDBType.Director } }, Actors = new List<Actor> { actor9 }, IMDBEntry = new IMDBEntry { Name = "The Matrix", Url = "https://www.imdb.com/title/tt0133093/", Type = IMDBType.Movie }, Seen = true, Score = 10, Year = "1999" };

            // Mock data for Logger
            Logger log1 = new Logger { Id = 25, TimeOfDay = DateTime.Now, UserId = 1, Description = "User login", Type = LogType.user };
            Logger log2 = new Logger { Id = 26, TimeOfDay = DateTime.Now, UserId = null, Description = "System error", Type = LogType.error };

            // Mock data for User
            User user1 = new User { Id = 27, Name = "John Doe", Email = "john.doe@example.com", PasswordHash = "hashed_password", DateOfCreation = DateTime.Now };
            User user2 = new User { Id = 28, Name = "Jane Smith", Email = "jane.smith@example.com", PasswordHash = "hashed_password", DateOfCreation = DateTime.Now };

            // Mock data for UserLog
            UserLog userLog1 = new UserLog { Id = 29, TimeOfDay = DateTime.Now, AmountOfUsers = 100 };
            UserLog userLog2 = new UserLog { Id = 30, TimeOfDay = DateTime.Now, AmountOfUsers = 150 };

            MovieRepository movieRepository = new MovieRepository();

            movieRepository.AddMovieByMovie(movie1);
            movieRepository.AddMovieByMovie(movie2);
            movieRepository.AddMovieByMovie(movie3);
            movieRepository.AddMovieByMovie(movie4);
            movieRepository.AddMovieByMovie(movie5);

            movieRepository.ShowAllMoviesAUserContains();

            movieRepository.RemoveMovieByMovie(movie1);
            
            AddSomeSpace();

            movieRepository.ShowAllMoviesAUserContains();

            Console.ReadLine();
        }

        static void AddSomeSpace()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------");
            Console.WriteLine();
        }
    }
}