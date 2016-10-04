using LuxtourOnline.Models;
using LuxtourOnline.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuxtourOnline.Utilites;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Identity;
using NLog;
using Microsoft.AspNet.Identity.Owin;

namespace LuxtourOnline.Repos
{
    public class UserRepo: BaseRepo, IDisposable
    {

        public async Task RemoveUser(string id)
        {
            var manager = _userManager;

            var current = GetCurrentUser();

            if (id == current.Id)
                throw new Exception($"User {current.FullName} try to kill himself!");

            var userToRemove = manager.Users.Where(x => x.Id == id).FirstOrDefault();

            if (userToRemove != null)
            {
                var roles = await manager.GetRolesAsync(userToRemove.Id);
                var currentUserRoles = await manager.GetRolesAsync(GetCurrentUser().Id);

                if (roles.Contains("admin") && !currentUserRoles.Contains("admin"))
                {
                    throw new FieldAccessException($"User {GetCurrentUser().FullName} try delete { manager.Users.Where(x => x.Id == id).FirstOrDefault().FullName}");
                }

                _log.Info($"User {GetCurrentUser().FullName} removed user {userToRemove.FullName} / {userToRemove.Email} with role: {GetUserRoles(userToRemove.Id)[0]}");

                await manager.DeleteAsync(userToRemove);
            }
            else
            {
                throw new KeyNotFoundException($"No user with id: {id}");
            }
        }

        public RemoveUserModel GetUserToRemove(string id)
        {
            var manager = _userManager;

            var user = manager.Users.Where(x => x.Id == id).First();
            RemoveUserModel model = new RemoveUserModel(user);

            return model;
        }

        public async Task<bool> ChangePassword(ChangePassword model)
        {
            var user = GetCurrentUser();

            if (model.Id !=user.Id)
                throw new AccessViolationException($"User {user.FullName} try to cheat! ( id: {user.Id})");

            var manager = _userManager;

            bool check = await manager.CheckPasswordAsync(user, model.OldPassword);

            if (!check)
                return false;

            var result = await manager.ChangePasswordAsync(user.Id, model.OldPassword, model.Password);

            if (result == IdentityResult.Success)
                return true;

            return false;
        }

        public async Task<DisplayUserModel> GetDisplayUser(string id)
        {
            var manager = _userManager;

            var user = manager.Users.Where(x => x.Id == id).FirstOrDefault();

            if (user == null)
                throw new InvalidDataException($"No user with id {id.ToString()}");


            var rolesList = await manager.GetRolesAsync(user.Id);
            var roles = string.Join(", ", rolesList);

            var model = new DisplayUserModel() { Email = user.Email, CreatedDate = user.RegDate, FullName = user.FullName, PhoneNumber = user.PhoneNumber, Id = user.Id, Roles = roles };

            return model;
        }



        public async Task<ListUserModel> UserList(int usersPerPage, int page, string email = "", string fullname = "", string role = "", string phone = "")
        {
            var manager = _userManager;

            var users = manager.Users.OrderBy(x => x.RegDate).ToList();

            if (email != "")
                users = users.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();

            if (fullname != "")
                users = users.Where(x => x.FullName.ToLower().Contains(fullname.ToLower())).ToList();

            if (phone != "")
                users = users.Where(x => x.PhoneNumber.ToLower().Contains(phone.ToLower())).ToList();

            users = users.Skip((page - 1) * usersPerPage).Take(usersPerPage).ToList();

            ListUserModel model = new ListUserModel();
            model.Paging = new PagingInfo() { CurrentPange = page, ItemsPerPage = usersPerPage, TotalItems = _context.Users.Count() };
            model.Users = new List<DisplayUserModel>();


            foreach (var user in users)
            {
                if (role != "" && !manager.GetRoles(user.Id).Contains(role.ToLower()))
                    continue;

                var roles = string.Join(" / ", await manager.GetRolesAsync(user.Id));

                model.Users.Add(new DisplayUserModel() { CreatedDate = user.RegDate, Email = user.Email, FullName = user.FullName, PhoneNumber = user.PhoneNumber,Id = user.Id, Roles = string.Join(" / ", roles) });
            }

            return model;
        }

        public async Task<bool> ChangeEmail(ChangeEmail model)
        {
            var user = GetCurrentUser();

            if (user.Id != model.Id)
                throw new FieldAccessException($"User {user.FullName} try to cheat");

            var manager = _userManager;

            bool s = await manager.CheckPasswordAsync(user, model.Password);

            if (!s)
                return false;

            user.Email = model.NewEmail;
            user.UserName = model.NewEmail;

            await manager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ChangeFullName(ChangeFullName model)
        {
            var user = GetCurrentUser();

            if (user.Id != model.Id)
                throw new FieldAccessException($"user {user.FullName} try to cheat");

            var manager = _userManager;

            bool s = await manager.CheckPasswordAsync(user, model.Password);

            if (!s)
                return false;

            user.FullName = model.FullName;

            await manager.UpdateAsync(user);

            return true;
        }

        public ChangePhoneNumber ChangePhoneModel()
        {
            var user = GetCurrentUser();

            var model = new ChangePhoneNumber(user);

            return model;
        }

        public async Task<bool> ChangePhoneNumber(ChangePhoneNumber model)
        {
            var user = GetCurrentUser();

            var manager = _userManager;

            bool result = await manager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return false;

            var token = await manager.GenerateChangePhoneNumberTokenAsync(user.Id, model.PhoneNumber);

            await manager.ChangePhoneNumberAsync(user.Id, model.PhoneNumber, token);

            return true;
        }

        public async Task UpdateUser(UpdateUserModel model)
        {
            var manager = _userManager;

            var user = manager.Users.Where(x => x.Id == model.Id).FirstOrDefault();

            if (user == null)
                throw new ArgumentNullException($"No user with id {model.Id}");

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email;

            if (model.NewRole != null)
            {
                var roles = _roleManager;

                if (roles.RoleExists(model.NewRole))
                {
                    if (!(model.NewRole == "admin" && !GetUserRoles().Contains("admin")))
                    {
                        foreach(var r in GetUserRoles(user.Id))
                        {
                            await manager.RemoveFromRoleAsync(user.Id, r);
                        }

                        await manager.AddToRoleAsync(user.Id, model.NewRole);

                    }
                }
            }

            await manager.UpdateAsync(user);
        }
    }
}