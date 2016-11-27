using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using PIMSuite.Persistence;
using System.Linq;
using PIMSuite.Utilities.Auth;

namespace UnitTestDataLayer
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IUserRepository ur = new UserRepository(new DataContext());
            //Testdaten
            IDepartmentRepository departmentRepository = new DepartmentRepository(new DataContext());
            ILocationRepository locationRepository = new LocationRepository(new DataContext());
            IUserRepository userRepository = new UserRepository(new DataContext());
            IMessageRepository mr = new MessageRepository(new DataContext());

            User u1 = new User
            {
                Firstname = "Anja",
                Lastname = "Pupkina",
                Username = "pupkina",
                Email = "pupkina@mail.org",
                PhoneNumber = "12345678",
                DepartmentName = "IT-Support",
                LocationName = "Berlin",
                Password = new HashHelper().Hash("mustermann"),
                isAdmin = true

            };
            User u2 = new User
            {
                Firstname = "John",
                Lastname = "Schmidt",
                Username = "schmidt",
                Email = "schmidt@mail.org",
                PhoneNumber = "12345678",
                DepartmentName = "IT-Support",
                LocationName = "Berlin",
                Password = new HashHelper().Hash("mustermann"),
                isAdmin = true

            };
            User u3 = new User
            {
                Firstname = "Katrin",
                Lastname = "Meier",
                Username = "meier",
                Email = "meier@mail.org",
                PhoneNumber = "12345678",
                DepartmentName = "IT-Support",
                LocationName = "Berlin",
                Password = new HashHelper().Hash("mustermann"),
                isAdmin = true

            };

            if (userRepository.GetUsers().Contains(u1) == false && userRepository.GetUsers().Contains(u2) == false && userRepository.GetUsers().Contains(u3) == false)
            {
                userRepository.InsertUser(u1);
                userRepository.InsertUser(u2);
                userRepository.InsertUser(u3);
                userRepository.Save();
            }

            Message m1 = new Message { SenderId = u1.UserId, ReceiverId = u2.UserId, MessageBody = "First" };
            Message m2 = new Message { SenderId = u2.UserId, ReceiverId = u1.UserId, MessageBody = "Second" };
            Message m3 = new Message { SenderId = u3.UserId, ReceiverId = u1.UserId, MessageBody = "Third to first" };

            DataContext cont = new DataContext();
            if (cont.Messages.ToList().Contains(m1) == false && cont.Messages.ToList().Contains(m2) == false && cont.Messages.ToList().Contains(m3) == false)
            {
                mr.InsertMessage(m1);
                mr.InsertMessage(m2);
                mr.InsertMessage(m3);
            }

            var cons = mr.GetConversationOfTwoUsers(u1.UserId, u2.UserId);
            foreach (Message m in cons)
            {
                Console.WriteLine(m.MessageBody);
            }

            mr.DeleteMessage(m1.MessageId);
            mr.DeleteMessage(m2.MessageId);
            mr.DeleteMessage(m3.MessageId);




        }
            
    }
}
