using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class StaffTests
    {
        [SetUp]
        public void SetUp()
        {
            Staff.SetAll(new List<Staff>());
            Staff.TotalStaff = 0;
        }

        //derived class for testing becuase Staff is abstract
        private class TestStaff : Staff
        {
            public TestStaff(int staffID, string name, string? shift = null)
                : base(staffID, name, shift) { }

            public TestStaff() { } // parameterless constructor for serialization
        }

        [Test]
        public void Constructor_ShouldInitializeStaffCorrectly()
        {
            var staff = new TestStaff(1, "John Doe", "Morning");

            Assert.That(staff.StaffID, Is.EqualTo(1));
            Assert.That(staff.Name, Is.EqualTo("John Doe"));
            Assert.That(staff.Shift, Is.EqualTo("Morning"));
            Assert.That(Staff.TotalStaff, Is.EqualTo(1));
        }

        [Test]
        public void Name_ShouldThrowException_WhenNullOrEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => new TestStaff(1, "", "Morning"));
            Assert.That(ex.Message, Is.EqualTo("Name cannot be null or empty."));
        }

        [Test]
        public void Shift_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longShift = new string('A', 51);
            var ex = Assert.Throws<ArgumentException>(() => new TestStaff(1, "John Doe", longShift));
            Assert.That(ex.Message, Is.EqualTo("Shift length cannot exceed 50 characters."));
        }


        [Test]
        public void RemoveStaff_ShouldThrowException_WhenStaffNotFound()
        {
            var staff1 = new TestStaff(1, "John Doe");
            

            Staff.RemoveStaff(staff1); // Should throw since staff1 was not added
            var ex = Assert.Throws<ArgumentException>(() => Staff.RemoveStaff(staff1));
            Assert.That(ex.Message, Is.EqualTo("Staff member not found."));
        }

        [Test]
        public void SetAll_ShouldUpdateStaffListCorrectly()
        {
            var staff1 = new TestStaff(1, "John Doe");
            var newStaffList = new List<Staff> { staff1 };
            Staff.SetAll(newStaffList);

            var allStaff = Staff.GetAll();
            Assert.That(allStaff.Count, Is.EqualTo(1));
            Assert.Contains(staff1, (System.Collections.ICollection)allStaff);
            Assert.That(Staff.TotalStaff, Is.EqualTo(1));
        }

        [Test]
        public void TotalStaff_ShouldThrowException_WhenSetToNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => Staff.TotalStaff = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalStaff cannot be negative."));
        }

        [Test]
        public void RemoveStaff_ShouldUpdateTotalStaffCount()
        {
            var staff = new TestStaff(1, "John Doe");
            Assert.That(Staff.TotalStaff, Is.EqualTo(1));

            Staff.RemoveStaff(staff);
            Assert.That(Staff.TotalStaff, Is.EqualTo(0));
        }

        [Test]
        public void GetAll_ShouldReturnAllStaffMembers()
        {
            var staff1 = new TestStaff(1, "John Doe");
            var staff2 = new TestStaff(2, "Jane Smith");

            var allStaff = Staff.GetAll();
            Assert.That(allStaff.Count, Is.EqualTo(2));
            Assert.Contains(staff1, (System.Collections.ICollection)allStaff);
            Assert.Contains(staff2, (System.Collections.ICollection)allStaff);
        }
    }
}
