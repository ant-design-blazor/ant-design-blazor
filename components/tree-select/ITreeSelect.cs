// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public interface ITreeSelect
    {
        internal void UpdateValueAfterDataSourceChanged();

        internal string GetSemanticClass(string slot, string baseClass = null);

        internal string GetSemanticStyle(string slot, string baseStyle = null);

        internal string GetPopupSemanticClass(string slot, string baseClass = null);

        internal string GetPopupSemanticStyle(string slot, string baseStyle = null);
    }
}
