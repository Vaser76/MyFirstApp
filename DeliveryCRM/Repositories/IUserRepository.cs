using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryCRM.Entities;

namespace DeliveryCRM.Repositories
{
    public interface IUserRepository
    {
        /// Получить список пользователей
        public IEnumerable<UserEntity> GetUsers();

        /// Добавить пользователя
        public string AddUser(UserEntity user);

        /// Обновить данные
        public void UpdateUser(string id, UserEntity user);

        /// Данные пользователя по идентификатору
        public UserEntity GetUsertById(string id);

        /// Удалить пользователя
        public void DeleteUser(string id);
    }
}
