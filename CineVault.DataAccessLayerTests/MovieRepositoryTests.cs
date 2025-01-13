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
            var movie = new Movie { Title = "No Barcode Movie", Barcode = null };

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

        #region AddMovieByBarcode - Testing

        #endregion

        #region RemoveMovieByMovie - Testing

        [TestMethod]



        #endregion

        #region RemoveMovieByBarcode - Testing

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
