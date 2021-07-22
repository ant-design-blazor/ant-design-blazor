export var FocusBehavior;
(function (FocusBehavior) {
    /// <summary>
    /// When focuses, cursor will move to the last character. 
    /// This is default behavior.
    /// </summary>
    FocusBehavior[FocusBehavior["FocusAtLast"] = 0] = "FocusAtLast";
    /// <summary>
    /// When focuses, cursor will move to the first character
    /// </summary>
    FocusBehavior[FocusBehavior["FocusAtFirst"] = 1] = "FocusAtFirst";
    /// <summary>
    /// When focuses, the content will be selected
    /// </summary>
    FocusBehavior[FocusBehavior["FocusAndSelectAll"] = 2] = "FocusAndSelectAll";
})(FocusBehavior || (FocusBehavior = {}));
