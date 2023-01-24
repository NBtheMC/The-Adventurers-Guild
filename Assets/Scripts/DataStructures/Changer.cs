[System.Serializable]
public struct Changer
{
    public string name; // Name of worldValue to change
    public float value; // Value to add (can be negative)
    public string valueFormulaString; // String of a mathematical formula that will be calculated into the value field
    public bool setOrAdd; // Should the value be set or add? T - Set, F - Add
}
