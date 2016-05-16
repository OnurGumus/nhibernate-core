namespace NHibernate.Test.ReadOnly
{
	public partial class StudentDto
	{
		private string studentName;
		private string courseDescription;
		
		public string Name
		{
			get { return studentName; }
		}
		
		public string Description
		{
			get { return courseDescription; }
		}
	}
}
