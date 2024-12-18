using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.AssociationTests
{
    [TestFixture]
    public class StaffReflexTests
    {
        private Bartender supervisor;
        private Bartender subordinate1;
        private Bartender subordinate2;

        [SetUp]
        public void Setup()
        {
            Bartender.SetAll(new List<Bartender>());
            Staff.SetAll(new List<Staff>());

            supervisor = new Bartender(1, "John Doe");
            subordinate1 = new Bartender(2, "Jane Smith");
            subordinate2 = new Bartender(3, "Alice Brown");
        }

        [Test]
        public void StartManaging_ValidStaff_AddsStaffToManagedList()
        {
            supervisor.startManaging(subordinate1);

            Assert.That(supervisor.GetManagedStaff().Contains(subordinate1));
            Assert.AreEqual(supervisor, subordinate1.GetSupervisor());
        }

        [Test]
        public void StartManaging_NullStaff_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => supervisor.startManaging(null));
        }

        [Test]
        public void StartManaging_AlreadyHasSupervisor_ThrowsInvalidOperationException()
        {
            var otherSupervisor = new Bartender(4, "Other Supervisor");
            otherSupervisor.startManaging(subordinate1);

            Assert.Throws<InvalidOperationException>(() => supervisor.startManaging(subordinate1));
        }

        [Test]
        public void SetSupervisor_ValidSupervisor_AssignsSupervisor()
        {
            subordinate1.setSupervisor(supervisor);

            Assert.AreEqual(supervisor, subordinate1.GetSupervisor());
            Assert.That(supervisor.GetManagedStaff().Contains(subordinate1));
        }

        [Test]
        public void SetSupervisor_SelfSupervision_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => supervisor.setSupervisor(supervisor));
        }

        [Test]
        public void RemoveManagedStaff_ValidStaff_RemovesStaffFromList()
        {
            supervisor.startManaging(subordinate1);
            supervisor.removeManagedStaff(subordinate1);

            Assert.That(supervisor.GetManagedStaff(), Is.Empty);
            Assert.IsNull(subordinate1.GetSupervisor());
        }

        [Test]
        public void RemoveSupervisor_RemovesSupervisorAssociation()
        {
            subordinate1.setSupervisor(supervisor);
            subordinate1.removeSupervisor();

            Assert.IsNull(subordinate1.GetSupervisor());
            Assert.That(supervisor.GetManagedStaff(), Is.Empty);
        }

        [Test]
        public void ModifyManagedStaff_ReplacesStaffCorrectly()
        {
            supervisor.startManaging(subordinate1);
            supervisor.modifyManagedStaff(subordinate1, subordinate2);

            Assert.That(supervisor.GetManagedStaff().Contains(subordinate2));
            Assert.That(supervisor.GetManagedStaff(), Does.Not.Contain(subordinate1));
            Assert.AreEqual(supervisor, subordinate2.GetSupervisor());
            Assert.IsNull(subordinate1.GetSupervisor());
        }

        [Test]
        public void ModifySupervisor_ReassignsSupervisorCorrectly()
        {
            var newSupervisor = new Bartender(5, "New Supervisor");
            subordinate1.setSupervisor(supervisor);
            subordinate1.modifySupervisor(newSupervisor);

            Assert.AreEqual(newSupervisor, subordinate1.GetSupervisor());
            Assert.That(supervisor.GetManagedStaff(), Is.Empty);
            Assert.That(newSupervisor.GetManagedStaff(), Has.Member(subordinate1));
        }
    }
}
