using Microsoft.VisualStudio.TestTools.UnitTesting;
using CallMeMaybeClient.ViewsModels;
using CallMeMaybeClient.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CallMeMaybeClient.Tests
{
    [TestClass]
    public class SalarieViewModelTests
    {
        private SalarieViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new SalarieViewModel();
        }

        [TestMethod]
        public async Task LoadDataAsync_ShouldLoadSalaries()
        {
            // Act
            await _viewModel.LoadDataAsync();

            // Assert
            Assert.IsNotNull(_viewModel.Salaries);
            Assert.IsTrue(_viewModel.Salaries.Count > 0);
        }

        [TestMethod]
        public void PerformSearch_ShouldFilterSalaries()
        {
            // Arrange
            _viewModel.Salaries = new ObservableCollection<Salarie>
            {
                new Salarie { nom = "John", prenom = "Doe" },
                new Salarie { nom = "Jane", prenom = "Smith" }
            };
            _viewModel.SearchText = "John";

            // Act
            _viewModel.PerformSearch();

            // Assert
            Assert.AreEqual(1, _viewModel.Salaries.Count);
            Assert.AreEqual("John", _viewModel.Salaries[0].nom);
        }

        [TestMethod]
        public void CanDelete_ShouldReturnTrue_WhenSelectedSalarieIsNotNull()
        {
            // Arrange
            _viewModel.SelectedSalarie = new Salarie();

            // Act
            var result = _viewModel.CanDelete();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanDelete_ShouldReturnFalse_WhenSelectedSalarieIsNull()
        {
            // Arrange
            _viewModel.SelectedSalarie = null;

            // Act
            var result = _viewModel.CanDelete();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
