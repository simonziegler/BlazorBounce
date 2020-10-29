namespace BlazorBounce
{
    /// <summary>
    /// Determines what attributes to splat from <see cref="SplatAttributes"/>. Can be specified with bitwise or, eg:
    /// <code>@attributes="<see cref="AttributesToSplat"/>(<see cref="IdClassAndStyleOnly"/> | <see cref="HtmlExcludingIdClassAndStyle"/>)"</code>
    /// </summary>
    internal enum SplatType : ushort
    {
        /// <summary>
        /// Return all attributes including class and style, also including values from <see cref="ClassMapper"/> and <see cref="StyleMapper"/>.
        /// </summary>
        All = 0xFFFF,

        /// <summary>
        /// Return only class and style values, which includes <see cref="ClassMapper"/> and <see cref="StyleMapper"/>.
        /// </summary>
        IdClassAndStyleOnly = 0x0001,

        /// <summary>
        /// Return only class and style values, which includes <see cref="ClassMapper"/> and <see cref="StyleMapper"/>.
        /// </summary>
        HtmlExcludingIdClassAndStyle = 0x0002,

        /// <summary>
        /// Return only class and style values, which includes <see cref="ClassMapper"/> and <see cref="StyleMapper"/>.
        /// </summary>
        EventsOnly = 0x0004,

        /// <summary>
        /// Return all attributes except class and style, also excluding <see cref="ClassMapper"/> and <see cref="StyleMapper"/>.
        /// </summary>
        ExcludeIdClassAndStyle = HtmlExcludingIdClassAndStyle | EventsOnly,

        /// <summary>
        /// Return all attributes except class and style, also excluding <see cref="ClassMapper"/> and <see cref="StyleMapper"/>.
        /// </summary>
        ExcludeHtmlExceptIdClassAndStyle = IdClassAndStyleOnly | EventsOnly,

        /// <summary>
        /// Return all attributes except class and style, also excluding <see cref="ClassMapper"/> and <see cref="StyleMapper"/>.
        /// </summary>
        ExcludeEvents = IdClassAndStyleOnly | HtmlExcludingIdClassAndStyle
    }
}
