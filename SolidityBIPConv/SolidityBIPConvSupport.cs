

public static class SolidityBIPSupport
{
    private string ReturnStringBtwSemiColons(string originalString)
    {
        string FinalString = "";

        for (int i = 0; i < originalString.Length; i++)
        {
            if (originalString[i] == '{')  // when current char is '{', start from current length 'i' and get all char storing it in one string
            {
                while (i < originalString.Length)
                {
                    FinalString += originalString[i];
                    i++;
                }
            }
        }
        return FinalString;
    }
}

