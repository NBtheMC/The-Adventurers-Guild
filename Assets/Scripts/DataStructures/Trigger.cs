[System.Serializable]
public struct Trigger
{
    public string name; // Name of worldValue to change
    public ValueType valueType; // Type to check against
    public float value; // Value to check against
    public string valueFormulaString; // String of a mathematical formula that will be calculated into the value field
    public NumberTriggerType comparisonType; // How the comparison should be performed
}