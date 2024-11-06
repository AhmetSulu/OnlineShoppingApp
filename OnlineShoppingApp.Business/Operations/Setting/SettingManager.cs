using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;

namespace OnlineShoppingApp.Business.Operations.Setting
{
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity> _settingRepository;

        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        //-----------------------------------------------------------------------------------------------------

        // Toggles the maintenance mode setting.
        public async Task ToggleMaintenance()
        {
            // Retrieve the setting entity with ID 1 (assumed to be the settings record).
            var setting = _settingRepository.GetById(1);

            // Toggle the MaintenanceMode property (true to false, or false to true).
            setting.MaintenanceMode = !setting.MaintenanceMode;
            _settingRepository.Update(setting); // Update the setting in the repository.

            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database.
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while updating the setting."); // Handle any exceptions.
            }
        }

        //-----------------------------------------------------------------------------------------------------

        // Gets the current state of maintenance mode.
        public bool GetMaintenanceState()
        {
            // Retrieve the setting entity and return its MaintenanceMode property.
            var maintenanceState = _settingRepository.GetById(1).MaintenanceMode;

            return maintenanceState; // Return the maintenance state (true or false).
        }
    }
}
