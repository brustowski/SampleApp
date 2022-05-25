namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest C4 detail
    /// </summary>
    public class C4Detail
    {
        /// <summary>
        /// Gets or sets the C4 number
        /// </summary>
        public string C4Number { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the Piece Count
        /// </summary>
        public string PieceCount { get; set; }
        /// <summary>
        /// Gets or sets the Piece Count Unit
        /// </summary>
        public string PieceCountUnit { get; set; }
        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country { get; set; }
    }
}
