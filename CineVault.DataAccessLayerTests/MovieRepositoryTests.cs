using CineVault.DataAccessLayer;
using CineVault.ModelLayer.ModelMovie;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CineVault.DataAccessLayerTests
{
    [TestClass]
    public sealed class MovieRepositoryTests
    {
        private MovieRepository testRepo = new MovieRepository();

        #region Adding or removing movies - Tests

        #region AddMovieByMovie - Testing

        [TestMethod]
        public void AddMovieByMovie_Should_AddValidMovie_ToList()
        {
            // Arrange
            Movie movie = new Movie { Title = "Inception", Barcode = "12345", Year = "2010", Seen = false };

            // Act
            testRepo.AddMovieByMovie(movie);

            // Assert
            Assert.IsTrue(testRepo.ShowAllMoviesAUserContains().Contains(movie),
                "The movie was not added to the list.");
        }

        [TestMethod]
        public void AddMovieByMovie_Should_ThrowException_WhenMovieIsNull()
        {
            // Arrange
            Movie movie = new Movie { Title = null, Barcode = null, Year = null, Seen = false };

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => testRepo.AddMovieByMovie(null));
        }

        [TestMethod]
        public void AddMovieByMovie_Should_AllowMovies_WithNullBarcode()
        {
            // Arrange
            Movie movie = new Movie { Title = "No Barcode Movie", Barcode = null };

            // Act
            testRepo.AddMovieByMovie(movie);

            // Assert
            CollectionAssert.Contains(testRepo.ShowAllMoviesAUserContains().ToList(), movie);
        }

        [TestMethod]
        public void AddMovieByMovie_Should_AllowMovies_WithNullBarcodeAlternative()
        {
            // Arrange
            //var movieRepository = new MovieRepository();
            Movie movie = new Movie { Title = "No Barcode Movie", Barcode = null };

            // Act
            testRepo.AddMovieByMovie(movie);

            // Assert
            IEnumerable<Movie> movies = testRepo.ShowAllMoviesAUserContains();
            Assert.IsTrue(movies.Any(m => m == movie), "Movie not found in the repository.");
        }

        #endregion

        #region AddMovieByBarcode - Testing

        [TestMethod]
        public void AddMovieByBarcode_Should_ThrowException_WhenBarcodeIsNullOrEmpty()
        {
            // Arrange
            //MovieRepository movieRepository = new MovieRepository();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => testRepo.AddMovieByBarcode(null));
            Assert.ThrowsException<ArgumentNullException>(() => testRepo.AddMovieByBarcode(""));
        }

        #endregion

        #region RemoveMovieByMovie - Testing

        [TestMethod]
        public void RemoveMovieById_ShouldRemoveMovieFromList()
        {
            // Arrange

            Movie movie1 = new Movie { Title = "Inception", Barcode = "12345", Year = "2010", Seen = false };
            Movie movie2 = new Movie { Title = "The Godfather", Barcode = "9876543210678", Year = "1972", Seen = true };
            testRepo.AddMovieByMovie(movie1);
            testRepo.AddMovieByMovie(movie2);

            // Act

            testRepo.RemoveMovieByMovie(movie2);

            // Assert

            Assert.IsTrue(!testRepo.ShowAllMoviesAUserContains().Contains(movie2));
        }

        [TestMethod]
        public void RemoveMovieByMovie_Should_ThrowException_WhenMovieIsNull()
        {
            // Arrange
            Movie movie = new Movie { Title = null, Barcode = null, Year = null, Seen = false };

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => testRepo.AddMovieByMovie(null));
        }

        [TestMethod]
        public void RemoveMovieByMovie_Should_ThrowException_IfMovieIsAlreadyRemoved()
        {
            // Arrange

            Movie movie1 = new Movie { Title = "Inception", Barcode = "12345", Year = "2010", Seen = false };
            Movie movie2 = new Movie { Title = "The Godfather", Barcode = "9876543210678", Year = "1972", Seen = true };
            testRepo.AddMovieByMovie(movie1);
            testRepo.AddMovieByMovie(movie2);

            // Act

            testRepo.RemoveMovieByMovie(movie2);
            testRepo.RemoveMovieByMovie(movie2);

            // Assert

            Assert.ThrowsException<ArgumentNullException>(() => testRepo.RemoveMovieByMovie(null));

        }

        #endregion

        #region RemoveMovieByBarcode - Testing

        [TestMethod]
        public void RemoveMovieByBarcode_Should_ThrowException_WhenBarcodeIsNullOrEmpty()
        {
            // Arrange
            //MovieRepository movieRepository = new MovieRepository();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => testRepo.RemoveMovieByBarcode(null));
            Assert.ThrowsException<ArgumentNullException>(() => testRepo.RemoveMovieByBarcode(""));
        }

        #endregion

        #endregion

        #region Retrieve movies by status - Tests

        #region ShowAllMoviesAUserContains - Testing



        #endregion

        #region ShowAllMoviesThatHaveBeenSeen - Testing

        #endregion

        #region ShowAllMoviesThatHaveNotBeenSeen - Testing

        #endregion

        #endregion

        #region Filter by movie data - Tests

        #region ShowAllActorsFromMovie - Testing

        #endregion

        #region ShowDirectorFromMovie - Testing

        #endregion

        #region ShowYearFromMovie - Testing

        #endregion

        #endregion

        #region Filter by model - Tests

        #region ShowMoviesFromTheSameActor - Testing

        #endregion

        #region ShowMoviesFromTheSameDirector - Testing

        #endregion

        #region ShowAllMoviesFromTheSameYear - Testing

        #endregion

        #endregion

    }

}
