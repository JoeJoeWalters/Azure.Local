namespace Azure.Local.Domain.Timesheets
{
    /// <summary>
    /// Type of work performed
    /// </summary>
    public enum WorkType
    {
        /// <summary>
        /// Regular working hours
        /// </summary>
        Regular = 0,

        /// <summary>
        /// Overtime hours
        /// </summary>
        Overtime = 1,

        /// <summary>
        /// Double-time hours
        /// </summary>
        DoubleTime = 2,

        /// <summary>
        /// Holiday work
        /// </summary>
        Holiday = 3,

        /// <summary>
        /// Paid time off
        /// </summary>
        PTO = 4,

        /// <summary>
        /// Sick leave
        /// </summary>
        Sick = 5,

        /// <summary>
        /// Training time
        /// </summary>
        Training = 6,

        /// <summary>
        /// Meeting time
        /// </summary>
        Meeting = 7
    }
}
