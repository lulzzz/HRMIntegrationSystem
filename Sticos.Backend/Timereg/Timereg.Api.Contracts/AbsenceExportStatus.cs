namespace Timereg.Api.Contracts
{

    public enum AbsenceExportStatus
    {
        Unknown = 0,
        /// <summary>
        /// The Absence has not yet been exported
        /// </summary>
        Initial = 5,
        /// <summary>
        /// The Absence has been processed successfully
        /// </summary>
        Success = 10,
        /// <summary>
        /// The Absence has been tried processed but failure occured
        /// </summary>
        Failed = 20,
        /// <summary>
        /// The Absence is obsolete and deleted externally. A new version of Absence is exported.
        /// </summary>
        Obsolete = 30,
    }
}
