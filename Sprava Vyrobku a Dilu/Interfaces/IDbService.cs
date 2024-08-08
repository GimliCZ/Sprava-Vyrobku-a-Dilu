using SpravaVyrobkuaDilu.Database.Models;

namespace SpravaVyrobkuaDilu.Interfaces
{
    public interface IDbService
    {
        /// <summary>
        /// Asynchronously adds a new dil to the database.
        /// </summary>
        /// <param name="dilModel">The dil model to add.</param>
        /// <returns><c>true</c> if the addition was successful; otherwise, <c>false</c>.</returns>
        Task<bool> AddDilModelAsync(DilModel dilModel);

        /// <summary>
        /// Asynchronously checks if there are any dil in the database.
        /// </summary>
        /// <returns><c>true</c> if there are any dil; otherwise, <c>false</c>.</returns>
        Task<bool> AnyDilyAsync();

        /// <summary>
        /// Asynchronously checks if there are any vyrobky in the database.
        /// </summary>
        /// <returns><c>true</c> if there are any vyrobky; otherwise, <c>false</c>.</returns>
        Task<bool> AnyVyrobkyAsync();

        /// <summary>
        /// Asynchronously deletes a dil from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the dil to delete.</param>
        /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteDilModelAsync(int id);

        /// <summary>
        /// Asynchronously deletes a vyrobky from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the vyrobky to delete.</param>
        /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteVyrobekModelByIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all vyrobky from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="List{VyrobekModel}"/> of all vyrobky.</returns>
        Task<List<VyrobekModel>> GetAllVyrobkyAsync();

        /// <summary>
        /// Asynchronously updates an existing dil in the database.
        /// </summary>
        /// <param name="dilModel">The dil model to update.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        Task<bool> UpdateDilModelAsync(DilModel dilModel);

        /// <summary>
        /// Asynchronously updates an existing vyrobky in the database.
        /// </summary>
        /// <param name="vyrobekModel">The vyrobky model to update.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        Task<bool> UpdateVyrobekModelAsync(VyrobekModel vyrobekModel);

        /// <summary>
        /// Asynchronously adds a new vyrobky and its associated dil to the database.
        /// </summary>
        /// <param name="vyrobekModel">The vyrobky model to add.</param>
        /// <param name="dilModels">The collection of dil models to associate with the vyrobky.</param>
        /// <returns><c>true</c> if the addition was successful; otherwise, <c>false</c>.</returns>
        Task<bool> AddVyrobekWithDilyAsync(VyrobekModel vyrobekModel, IEnumerable<DilModel> dilModels);
    }
}