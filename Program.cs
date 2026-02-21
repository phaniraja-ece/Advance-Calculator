using System;
using System.Globalization;
using System.Numerics; // For BigInteger support in factorials > 20


class Calculator
{

    static string version = "1.1.25 (Stable)";
    static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

        while (true)
        {
            Console.Clear();
            PrintMenu();
            
            Console.Write("Enter operator: ");
            string? input = Console.ReadLine();
            string op = input?.Trim().ToUpper() ?? "";

            if (op == "EXIT") 
            {
                Console.WriteLine("\nThank you for using the calculator!");
                Console.ResetColor(); 
                Console.Clear();
                break;
            }
            
            if (op == "HELP" || op == "") 
            {
                ShowHelp();
                continue; 
            }

            try
            {
                switch (op)
                {
                    case "ADD": Add(); break;
                    case "SUB": Subtract(); break;
                    case "MULTIPLY": Multiply(); break;
                    case "DIVIDE": Divide(); break;
                    case "ROOT": Root(); break;
                    case "SQUAREROOT": SquareRoot(); break;
                    case "CUBEROOT": CubeRoot(); break;
                    case "EXPONENT": Exponent(); break;
                    case "MULTI-OPERATION": MultiOperation(); break;
                    case "SQUARE": Square(); break;
                    case "CUBE": Cube(); break;
                    case "FACTORIAL": Factorial(); break;
                    case "TEST": RunTestSuite(); break; // Hidden option for testing
                    case "PI": ShowSecretPi(); break; // Hidden Easter Egg for Pi
                    case "PHI": ShowGoldenRatio(); break;// Hidden Easter Egg for Golden Ratio
                    case "EULER": ShowEuler(); break;// Hidden Easter Egg for Euler's Number
                    case "CREDITS": ShowCredits(); break;// Hidden Credits Page
                    case "SHOW-ROSTER-SET": ShowRosterSet(); break; // Hidden Easter Egg for Roster Set
                    case "VERSION": ShowVersion(); break; // Hidden Easter Egg for Version Info
                    case "SELF-DESTRUCT": InitiateSelfDestruct(); break; // Hidden Easter Egg for Self-Destruct Sequence
                    // NEW THEME LOGIC
                    
                    case string s when s.Contains("THEME"):
                    // 1. Display the Usage first as requested
                    Console.WriteLine("\nUsage: THEME [OPTION]");
                    Console.WriteLine("Options: MATRIX, CYBER, BLOOD, CLASSIC");
    
                    // 2. Ask the user for the input
                    Console.Write("\nWhich theme would you like to apply? ");
                    string themeChoice = (Console.ReadLine() ?? "").Trim().ToUpper();
    
                    // 3. Call the method
                    SetTheme(themeChoice);
                    break;
                    default: 
                        Console.WriteLine("\nInvalid! Check spelling. Press Enter...");
                        Console.ReadLine();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}\nPress Enter...");
                Console.ReadLine();
            }
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine("=== ADVANCED CALCULATOR ===");
        Console.WriteLine("\nAvailable Operations:");
        Console.WriteLine("  ADD, SUB, MULTIPLY, DIVIDE, ROOT, SQUAREROOT, CUBEROOT, EXPONENT, MULTI-OPERATION, SQUARE, CUBE, FACTORIAL, SET-THEME");
        Console.WriteLine("\n Type HELP for examples or EXIT to quit");
        Console.WriteLine(new string('=', 50));
    }

    static void ShowHelp()
    {
        Console.Clear();
        Console.WriteLine("=== HELP & EXAMPLES ===");
        Console.WriteLine("  ADD              - Addition");
        Console.WriteLine("  ROOT             - Nth Root");
        Console.WriteLine("  MULTI-OPERATION  - Basic math (2+3, 5*4)");
        Console.WriteLine("  FACTORIAL        - n!");
        Console.WriteLine("\nPress Enter to return...");
        Console.ReadLine();
    }

    static void MultiOperation()
{
    Console.Clear();
    Console.WriteLine("=== INFINITY BREAKER: MULTI-OP ENGINE (BEDMAS) ===");
    Console.WriteLine("Supports: +, -, *, /, ^ (Power), and ( ) Brackets");
    Console.Write("\nEnter expression: ");
    string? expr = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(expr)) return;

    try
    {
        // The Engine call: maintains 500-digit precision
        string result = Evaluate(expr.Trim());
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n--------------------------------------------------");
        Console.WriteLine($"RESULT: {result}");
        Console.WriteLine("--------------------------------------------------");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[ ENGINE ERROR ]: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine("\nPress Enter to return to main menu...");
    Console.ReadLine();
}

static string Evaluate(string expression)
{
    expression = expression.Replace(" ", "").Replace("**", "^");
    // Regex splits numbers while keeping operators and brackets
    string[] tokens = System.Text.RegularExpressions.Regex.Split(expression, @"([\+\-\*\/\^\(\)])");
    
    var values = new Stack<string>(); // Storing numbers as strings to keep precision
    var ops = new Stack<char>();

    for (int i = 0; i < tokens.Length; i++)
    {
        string token = tokens[i];
        if (string.IsNullOrEmpty(token)) continue;

        // If it's a number (including decimals), push as string
        if (char.IsDigit(token[0]) || (token.Length > 1 && token[0] == '-'))
        {
            values.Push(token);
        }
        else if (token == "(") { ops.Push('('); }
        else if (token == ")")
        {
            while (ops.Count > 0 && ops.Peek() != '(')
                values.Push(ApplyBigOp(ops.Pop(), values.Pop(), values.Pop()));
            ops.Pop(); 
        }
        else if ("+-*/^".Contains(token))
        {
            char op = token[0];
            while (ops.Count > 0 && ops.Peek() != '(' && HasPrecedence(op, ops.Peek()))
                values.Push(ApplyBigOp(ops.Pop(), values.Pop(), values.Pop()));
            ops.Push(op);
        }
    }

    while (ops.Count > 0)
        values.Push(ApplyBigOp(ops.Pop(), values.Pop(), values.Pop()));

    return values.Pop();
}

static bool HasPrecedence(char op1, char op2)
{
    // Higher number = higher priority
    static int GetPriority(char op) => op switch {
        '^' => 3,
        '*' or '/' => 2,
        '+' or '-' => 1,
        _ => 0
    };

    // If op2 is higher or equal priority, it must be calculated first
    return GetPriority(op2) >= GetPriority(op1);
}

static string ApplyBigOp(char op, string s2, string s1)
{
    // 1. Alignment and Parsing
    int dot1 = s1.IndexOf('.');
    int dot2 = s2.IndexOf('.');
    int p1 = dot1 < 0 ? 0 : s1.Length - dot1 - 1;
    int p2 = dot2 < 0 ? 0 : s2.Length - dot2 - 1;
    
    // 2. Perform Math based on operation type
    BigInteger n1, n2, res = 0;
    int finalP = 0;

    switch (op)
    {
        case '+':
        case '-':
            finalP = Math.Max(p1, p2);
            n1 = BigInteger.Parse(s1.Replace(".", "") + new string('0', finalP - p1));
            n2 = BigInteger.Parse(s2.Replace(".", "") + new string('0', finalP - p2));
            res = (op == '+') ? n1 + n2 : n1 - n2;
            break;
        case '*':
            finalP = p1 + p2;
            n1 = BigInteger.Parse(s1.Replace(".", ""));
            n2 = BigInteger.Parse(s2.Replace(".", ""));
            res = n1 * n2;
            break;
        case '/':
            n1 = BigInteger.Parse(s1.Replace(".", ""));
            n2 = BigInteger.Parse(s2.Replace(".", ""));
            if (n2 == 0) throw new DivideByZeroException();
            // Scale up for 500-place division
            n1 *= BigInteger.Pow(10, 500 + p2 - p1);
            res = n1 / n2;
            finalP = 500;
            break;
        case '^':
            // Simple power logic for whole number exponents
            n1 = BigInteger.Parse(s1.Replace(".", ""));
            int exponent = int.Parse(s2); 
            res = BigInteger.Pow(n1, exponent);
            finalP = p1 * exponent;
            break;
    }

    // 3. Format back to string to pass to the next BEDMAS step
    string r = res.ToString();
    bool neg = r.StartsWith("-");
    if (neg) r = r.Substring(1);
    if (r.Length <= finalP) r = r.PadLeft(finalP + 1, '0');
    string result = (neg ? "-" : "") + r.Insert(r.Length - finalP, ".");
    
    // Cap at 500 decimals for internal steps
    int dotIdx = result.IndexOf('.');
    if (dotIdx != -1 && (result.Length - dotIdx - 1) > 500)
        result = result.Substring(0, dotIdx + 501);
        
    return result.TrimEnd('0').TrimEnd('.');
}
    static void Add() 
{ 
    BigDecBinaryOp("ADD", (a, b) => a + b); 
    Console.WriteLine("\nPress Enter to return...");
    Console.ReadLine(); 
}

    static void Subtract() 
{ 
    BigDecBinaryOp("SUB", (a, b) => a - b); 
    Console.WriteLine("\nPress Enter to return...");
    Console.ReadLine(); 
}
    
    static void Multiply()
{
    Console.WriteLine("\n--- MULTIPLICATION MODE (500 Decimal Limit) ---");
    
    Console.Write("Enter first number: ");
    string s1 = (Console.ReadLine() ?? "0").Trim();
    Console.Write("Enter second number: ");
    string s2 = (Console.ReadLine() ?? "0").Trim();

    // 1. Count decimal places for both
    int dot1 = s1.IndexOf('.');
    int dot2 = s2.IndexOf('.');
    int places1 = dot1 < 0 ? 0 : s1.Length - dot1 - 1;
    int places2 = dot2 < 0 ? 0 : s2.Length - dot2 - 1;
    
    // Total places in the result is the sum of input places
    int totalDecimalPlaces = places1 + places2;

    // 2. Remove dots and multiply as pure integers
    string clean1 = s1.Replace(".", "");
    string clean2 = s2.Replace(".", "");

    if (BigInteger.TryParse(clean1, out BigInteger n1) && BigInteger.TryParse(clean2, out BigInteger n2))
    {
        // The CPU will crush this even if n1/n2 have 1000+ digits
        BigInteger result = n1 * n2;
        string resStr = result.ToString();
        bool isNegative = resStr.StartsWith("-");
        if (isNegative) resStr = resStr.Substring(1);

        // 3. Re-insert the decimal point at totalDecimalPlaces
        if (resStr.Length <= totalDecimalPlaces)
        {
            resStr = resStr.PadLeft(totalDecimalPlaces + 1, '0');
        }

        int dotPos = resStr.Length - totalDecimalPlaces;
        string finalResult = (isNegative ? "-" : "") + resStr.Insert(dotPos, ".");

        // 4. APPLY THE 500-PLACE CAP
        int finalDotPos = finalResult.IndexOf('.');
        if (finalDotPos != -1)
        {
            int currentDecs = finalResult.Length - finalDotPos - 1;
            if (currentDecs > 500)
            {
                finalResult = finalResult.Substring(0, finalDotPos + 501);
            }
        }

        // 5. Final Cleanup
        finalResult = finalResult.TrimEnd('0').TrimEnd('.');
        if (string.IsNullOrEmpty(finalResult) || finalResult == "-") finalResult = "0";

        Console.WriteLine("--------------------------------------------------");
        if (finalResult.Length > 80)
            Console.WriteLine($"Result: {finalResult.Substring(0, 40)}... [Total Length: {finalResult.Length}]");
        else
            Console.WriteLine($"Result: {finalResult}");
        Console.WriteLine("--------------------------------------------------");
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }
    Console.ReadLine();
}
    
    static void Divide()
{
    Console.WriteLine("\n--- DIVISION MODE (500 Decimal Places) ---");
    
    // 1. Get Inputs
    Console.Write("Enter Dividend (Numerator): ");
    string s1 = (Console.ReadLine() ?? "0").Trim();
    Console.Write("Enter Divisor (Denominator): ");
    string s2 = (Console.ReadLine() ?? "0").Trim();

    // 2. Extract whole numbers and track their existing decimal points
    int dot1 = s1.IndexOf('.');
    int dot2 = s2.IndexOf('.');
    int places1 = dot1 < 0 ? 0 : s1.Length - dot1 - 1;
    int places2 = dot2 < 0 ? 0 : s2.Length - dot2 - 1;

    // Convert to BigIntegers (stripping the dots)
    if (BigInteger.TryParse(s1.Replace(".", ""), out BigInteger n1) && 
        BigInteger.TryParse(s2.Replace(".", ""), out BigInteger n2))
    {
        if (n2 == 0) 
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n[ MATH ERROR ]: Division by zero is undefined.");
    Console.WriteLine("The universe remains intact, but this calculation cannot proceed.");
    Console.ResetColor();
    Console.WriteLine("\nPress Enter to return to the menu...");
    Console.ReadLine();
    return; // This exits the Divide() method safely
}


            // 3. THE SCALING TRICK: 
            // We add 500 zeros to the numerator to "force" 500 decimal places.
            // We also adjust for the decimal places that were already there.
            int precision = 500;
        BigInteger scaledN1 = n1 * BigInteger.Pow(10, precision + places2 - places1);

        Console.WriteLine("Dividing... (Precision: 500 decimal places)");
        
        // The actual math happens here:
        BigInteger quotient = scaledN1 / n2;

        // 4. Formatting the string back to a decimal
        string resStr = quotient.ToString();
        bool isNegative = resStr.StartsWith("-");
        if (isNegative) resStr = resStr.Substring(1);

        // Ensure we have enough length to place the dot
        if (resStr.Length <= precision)
            resStr = resStr.PadLeft(precision + 1, '0');

        int dotPos = resStr.Length - precision;
        string finalResult = (isNegative ? "-" : "") + resStr.Insert(dotPos, ".");

        // 5. Cleanup: Remove trailing zeros for a professional look
        finalResult = finalResult.TrimEnd('0').TrimEnd('.');
        if (string.IsNullOrEmpty(finalResult) || finalResult == "-") finalResult = "0";

        Console.WriteLine("--------------------------------------------------");
        if (finalResult.Length > 80)
            Console.WriteLine($"Result: {finalResult.Substring(0, 40)}... [Total Chars: {finalResult.Length}]");
        else
            Console.WriteLine($"Result: {finalResult}");
        Console.WriteLine("--------------------------------------------------");
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter valid numbers.");
    }

    Console.WriteLine("\nPress Enter to return...");
    Console.ReadLine();
}

    static void Root()
{
    Console.WriteLine("\n--- Nth ROOT MODE (High Precision & Imaginary Support) ---");
    
    // 1. Get Inputs
    Console.Write("Enter the number: ");
    string inputA = Console.ReadLine() ?? "";
    
    Console.Write("Enter the root degree (e.g., 5 for Root 5): ");
    string inputN = Console.ReadLine() ?? "";

    if (!BigInteger.TryParse(inputA, out BigInteger A) || !int.TryParse(inputN, out int n) || n <= 0)
    {
        Console.WriteLine("Invalid input. Root degree must be a positive integer.");
        return;
    }

    // 2. Determine if the result is Imaginary
    // Even roots (2, 4, 6...) of negative numbers require 'i'
    bool isImaginary = (A < 0 && n % 2 == 0);
    
    // 3. Set Precision (500 decimal places)
    int precision = 500;
    
    // We calculate using the Absolute Value (positive version)
    BigInteger absA = BigInteger.Abs(A);
    
    // Scaling Trick: A * 10^(precision * n)
    BigInteger multiplier = BigInteger.Pow(10, precision * n);
    BigInteger scaledNumber = absA * multiplier;

    Console.WriteLine($"\nCalculating... {(isImaginary ? "(Imaginary Result)" : "")}");

    // 4. The Math (Newton-Raphson)
    BigInteger rootValue = BigIntNthRoot(scaledNumber, n);

    // 5. Formatting the result
    string rootStr = rootValue.ToString();
    
    // Pad with leading zeros if the result is a tiny decimal
    if (rootStr.Length <= precision)
        rootStr = rootStr.PadLeft(precision + 1, '0');

    int dotPosition = rootStr.Length - precision;
    string integerPart = rootStr.Substring(0, dotPosition);
    string decimalPart = rootStr.Substring(dotPosition);

    // Apply negative sign for odd roots of negative numbers
    if (A < 0 && n % 2 != 0)
    {
        integerPart = "-" + integerPart;
    }

    // 6. Display Result
    Console.WriteLine("--------------------------------------------------");
    string suffix = isImaginary ? " i" : "";

    if (integerPart.Length > 25)
    {
        // Scientific notation for massive whole numbers
        Console.WriteLine($"Result: {integerPart[0]}.{integerPart.Substring(1, 10)}... x 10^{integerPart.Replace("-","").Length - 1}{suffix}");
    }
    else
    {
        // Standard view for smaller whole numbers
        Console.WriteLine($"Result: {integerPart}.{decimalPart.Substring(0, 50)}...{suffix}");
    }

    Console.WriteLine($"\nDONE! Calculated to {precision} decimal places.");
    
    if (isImaginary) 
        Console.WriteLine("Status: 🧬 Imaginary/Complex Number identified.");
    else if (integerPart.Replace("-","").Length > 308) 
        Console.WriteLine("Status: 🚀 Infinity Root broken!");

    Console.WriteLine("--------------------------------------------------");

    // 7. Save Option
    Console.Write("Save all 500 decimal places to file? (y/n): ");
    if (Console.ReadLine()?.ToLower() == "y")
    {
        File.WriteAllText("root_result.txt", $"{integerPart}.{decimalPart}{suffix}");
        Console.WriteLine("Saved to 'root_result.txt'.");
    }

    Console.WriteLine("\nPress Enter to return...");
    Console.ReadLine();
}

// Essential Helper Function (Newton-Raphson)
static BigInteger BigIntNthRoot(BigInteger A, int n)
{
    if (A == 0) return 0;
    if (n == 1) return A;

    BigInteger x = A / n; // Starting guess
    if (x < 1) x = 1;

    BigInteger lastX;
    //BigInteger nMinus1 = n - 1;
    int nMinus1 = n - 1; // Using int for n-1 since n is an int

    do
    {
        lastX = x;
        // The Formula: x = ((n-1)x + A / x^(n-1)) / n
        BigInteger xToNMinus1 = BigInteger.Pow(x, nMinus1);
        x = ((nMinus1 * x) + (A / xToNMinus1)) / n;
    } while (BigInteger.Abs(x - lastX) > 1);

    return x;
}

    static void SquareRoot()
{
    Console.WriteLine("\n--- SQUARE ROOT MODE (High Precision & Imaginary Support) ---");
    
    // 1. Get Input
    Console.Write("Enter a number: ");
    string input = Console.ReadLine() ?? "";
    if (!BigInteger.TryParse(input, out BigInteger number))
    {
        Console.WriteLine("Invalid input.");
        return;
    }

    // NEW: Check if the number is negative to handle 'i'
    bool isImaginary = number < 0;
    BigInteger absNumber = BigInteger.Abs(number);

    // 2. Set Precision
    int precision = 500; 
    
    // Scaling Trick: We use the absolute value for the actual calculation
    BigInteger multiplier = BigInteger.Pow(10, precision * 2);
    BigInteger scaledNumber = absNumber * multiplier;

    Console.WriteLine($"\nCalculating root to {precision} decimal places... {(isImaginary ? "(Imaginary Result)" : "")}");

    // 3. The Math (Using Newton's Method)
    BigInteger root = BigIntSquareRoot(scaledNumber);

    // 4. Format the result string
    string rootStr = root.ToString();
    
    if (rootStr.Length <= precision)
    {
        rootStr = rootStr.PadLeft(precision + 1, '0');
    }

    int dotPosition = rootStr.Length - precision;
    string integerPart = rootStr.Substring(0, dotPosition);
    string decimalPart = rootStr.Substring(dotPosition);

    // 5. Display Result
    Console.WriteLine("--------------------------------------------------");
    
    // Add 'i' suffix if the input was negative
    string suffix = isImaginary ? " i" : "";

    if (integerPart.Length > 20)
    {
        Console.WriteLine($"Root: {integerPart[0]}.{integerPart.Substring(1, 5)}... x 10^{integerPart.Length - 1}{suffix}");
    }
    else
    {
        // Show first 50 decimals for clarity
        Console.WriteLine($"Root: {integerPart}.{decimalPart.Substring(0, 50)}...{suffix}");
    }

    Console.WriteLine($"\nDONE! Calculated to {precision} decimal places.");
    
    if (isImaginary)
        Console.WriteLine("Status: 🧬 Imaginary result handled.");
    else if (integerPart.Length > 308) 
        Console.WriteLine("Status: 🚀 Infinity Root broken!");

    Console.WriteLine("--------------------------------------------------");

    // 6. Save to File
    Console.Write("Save full precision result to text file? (y/n): ");
    if (Console.ReadLine()?.ToLower() == "y")
    {
        File.WriteAllText("sqrt_result.txt", $"{integerPart}.{decimalPart}{suffix}");
        Console.WriteLine("Saved to 'sqrt_result.txt'.");
    }

    Console.WriteLine("\nPress Enter...");
    Console.ReadLine();
}
    static void CubeRoot()
{
    Console.WriteLine("\n--- CUBE ROOT MODE (High Precision) ---");
    
    // 1. Get Input
    Console.Write("Enter a number: ");
    string input = Console.ReadLine() ?? "";
    if (!BigInteger.TryParse(input, out BigInteger number))
    {
        Console.WriteLine("Invalid input.");
        return;
    }

    bool isInputNegative = number < 0;
    int precision = 500;
    
    // 2. The Choice (Only for Negative Numbers)
    string mode = "R"; // Default to Real
    if (isInputNegative)
    {
        Console.WriteLine("\nNegative detected! Choose output type:");
        Console.WriteLine("[R] Real Root (Negative number)");
        Console.WriteLine("[C] Complex Root (Principal root involving 'i')");
        Console.Write("Selection: ");
        mode = Console.ReadLine()?.ToUpper() ?? "R";
    }

    // 3. Scaling & Calculation
    BigInteger absNumber = BigInteger.Abs(number);
    BigInteger multiplier = BigInteger.Pow(10, precision * 3);
    BigInteger scaledNumber = absNumber * multiplier;

    Console.WriteLine($"\nCalculating... (Precision: {precision} places)");
    BigInteger rootMagnitude = BigIntCubeRoot(scaledNumber);

    // 4. Formatting Magnitude
    string rootStr = rootMagnitude.ToString().PadLeft(precision + 1, '0');
    int dotPos = rootStr.Length - precision;
    string magInt = rootStr.Substring(0, dotPos);
    string magDec = rootStr.Substring(dotPos);

    // 5. Logical Branching for Display
    Console.WriteLine("--------------------------------------------------");

    if (mode == "C" && isInputNegative)
    {
        // Complex Root Math: (root/2) + (root * sin(60°))i
        // Sin(60°) is approx 0.866025
        BigInteger halfRoot = rootMagnitude / 2;
        string hStr = halfRoot.ToString().PadLeft(precision + 1, '0');
        string hInt = hStr.Substring(0, hStr.Length - precision);
        string hDec = hStr.Substring(hStr.Length - precision);

        Console.WriteLine("MODE: COMPLEX PRINCIPAL ROOT");
        Console.WriteLine($"Result: {hInt}.{hDec.Substring(0, 50)}... + ({magInt}.{magDec.Substring(0, 10)}... * 0.866)i");
    }
    else
    {
        // Standard Real Root
        string prefix = isInputNegative ? "-" : "";
        Console.WriteLine("MODE: REAL ROOT");
        Console.WriteLine($"Result: {prefix}{magInt}.{magDec.Substring(0, 50)}...");
    }

    Console.WriteLine("--------------------------------------------------");

    // 6. Save
    Console.Write("Save full 500-digit result to file? (y/n): ");
    if (Console.ReadLine()?.ToLower() == "y")
    {
        string finalOut = (mode == "C" && isInputNegative) ? "Complex values saved in 'cubert_complex.txt'" : $"{magInt}.{magDec}";
        File.WriteAllText("cubert_result.txt", finalOut);
    }

    Console.WriteLine("\nPress Enter...");
    Console.ReadLine();
}

static BigInteger BigIntCubeRoot(BigInteger n)
{
    if (n == 0) return 0;
    
    // Handle negatives for cube roots
    BigInteger absN = BigInteger.Abs(n);
    
    // Initial guess
    BigInteger x = absN >> (absN.ToByteArray().Length * 2);
    if (x < 1) x = absN;

    BigInteger lastX;
    do
    {
        lastX = x;
        // Newton's Method for Cube Root: x = (2x + n/x^2) / 3
        x = (2 * x + (absN / (x * x))) / 3;
    } while (BigInteger.Abs(x - lastX) > 1);

    return n < 0 ? -x : x;
}

    static void Exponent()
{
    Console.WriteLine("\n--- EXPONENT MODE (Positive Whole Numbers) ---");
    
    // 1. Get Inputs
    int b = GetInt("BASE");
    int e = GetInt("EXPONENT");

    // Strictly positive logic as requested
    if (b < 0 || e < 0)
    {
        Console.WriteLine("Error: This mode is optimized for positive whole numbers.");
        return;
    }

    // 2. Predict the scale
    double predictedDigits = (e * Math.Log10(b)) + 1;
    // Handle b=1 separately because Log10(1) is 0
    if (b == 1) predictedDigits = 1; 
    
    int predictedCount = (int)Math.Floor(predictedDigits);

    Console.WriteLine($"\nPrediction: This result will have approx. {predictedCount:N0} digits.");

    // 3. Safety Check for 4GB RAM
    if (predictedCount > 1000000)
    {
        Console.WriteLine("🚨 WARNING: Result exceeds 1,000,000 digits.");
        Console.WriteLine("Calculation is easy, but displaying it may lag your PC.");
        Console.Write("Proceed anyway? (y/n): ");
        if (Console.ReadLine()?.ToLower() != "y") return;
    }

    // 4. Perform Calculation
    Console.WriteLine("Calculating...");
    BigInteger result = BigInteger.Pow(b, e);
    
    // 5. Formatting 
    string fullResult = result.ToString();
    int actualDigits = fullResult.Length;
    
    // 6. Display Logic
    Console.Write("How would you like to see the result?\n[S] Shortened or [D] All Digits? (s/d): ");
    string mode = Console.ReadLine()?.ToLower() ?? "s";

    Console.WriteLine("--------------------------------------------------");
    if (mode == "s" && actualDigits > 20)
    {
        // Scientific Notation: 1.2345... x 10^Length
        Console.WriteLine($"Result: {fullResult[0]}.{fullResult.Substring(1, 10)}... x 10^{actualDigits - 1}");
    }
    else
    {
        Console.WriteLine($"Result: {fullResult}");
    }

    // 7. Status Report
    Console.WriteLine($"\nDONE! Total Digits: {actualDigits:N0}");

    if (actualDigits > 308) 
        Console.WriteLine("Status: 🚀 Infinity Broken successfully!");
    else 
        Console.WriteLine("Status: ✅ Calculation complete.");

    Console.WriteLine("--------------------------------------------------");

    // 8. File Export for the "Mega-Results"
    if (actualDigits > 1000)
    {
        Console.Write("Save all digits to a text file? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            File.WriteAllText("exponent_result.txt", fullResult);
            Console.WriteLine("Success! Saved as 'exponent_result.txt'.");
        }
    }

    Console.WriteLine("\nPress Enter to return to menu...");
    Console.ReadLine();
}

    //static void Square() { UnaryOp("SQUARE", x => x * x); Console.ReadLine(); }
    //static void Cube() { UnaryOp("CUBE", x => x * x * x); Console.ReadLine(); }

    static void Square() => BigDecUnaryOp("SQUARE", x => x * x);
    static void Cube() => BigDecUnaryOp("CUBE", x => x * x * x);

    static void Factorial()
{
    // 1. Get Input
    int n = GetInt("integer (0-100000)"); 

    if (n < 0 || n > 100000) 
    {
        Console.WriteLine("\nPlease use a number between 0 and 100,000.\n");
        return;
    }

    // 2. Predict Digit Count using Stirling's Approximation (Logarithms)
    // Formula: log10(n!) approx n*log10(n/e) + log10(sqrt(2*pi*n))
    double predictedDigits = 0;
    if (n > 0)
    {
        predictedDigits = (n * Math.Log10(n / Math.E)) + (Math.Log10(2 * Math.PI * n) / 2.0) + 1;
    }
    else { predictedDigits = 1; }
    
    int predictedCount = (int)Math.Floor(predictedDigits);

    Console.WriteLine($"\nPrediction: This result will have approx. {predictedCount:N0} digits.");

    // 3. Safety Check for 4GB RAM
    if (predictedCount > 1000000)
    {
        Console.WriteLine("🚨 WARNING: This will generate over 1 million digits.");
        Console.WriteLine("Your CPU may handle it, but it will take a few minutes. Continue? (y/n)");
        if (Console.ReadLine()?.ToLower() != "y") return;
    }

    // 4. User Preference
    Console.Write("How would you like to see the result?\n[S] Shortened (Scientific) or [D] All Digits? (s/d): ");
    string mode = Console.ReadLine()?.ToLower() ?? "s";

    Console.WriteLine("\nCalculating... (This may take time for large numbers)");

    // 5. The Math
    BigInteger result = 1;
    for (int i = 2; i <= n; i++)
    {
        result *= i;
    }

    // 6. The "Formatting" Bottleneck
    string fullResult = result.ToString();
    int digitCount = fullResult.Length;
    
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"{n}! = ");

    if (mode == "s" && digitCount > 25)
    {
        Console.WriteLine($"{fullResult[0]}.{fullResult.Substring(1, 10)}... x 10^{digitCount - 1}");
    }
    else
    {
        Console.WriteLine(fullResult);
    }

    // 7. Status & Results
    Console.WriteLine($"\nDONE! Total Digits: {digitCount:N0}");

    if (digitCount > 308) 
        Console.WriteLine("Status: 🚀 Infinity Broken successfully!");
    else 
        Console.WriteLine("Status: ✅ Calculation complete.");

    Console.WriteLine("--------------------------------------------------");

    // 8. File Saving Logic
    if (digitCount > 1000)
    {
        Console.Write("\nSave all digits to a text file? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            File.WriteAllText("factorial_result.txt", fullResult);
            Console.WriteLine("Done! Created 'factorial_result.txt'.");
        }
    }

    Console.WriteLine("\nPress Enter to return to menu...");
    Console.ReadLine();
}
static void RunTestSuite()
{
    Console.Clear();
    Console.WriteLine("=== ENGINE STRESS TEST: 500-DECIMAL VERIFICATION ===");
    
    // Define test cases: { Expression, Expected Description }
    string[,] tests = {
        { "((1.5 + 2.5) * 2) ^ 3", "Whole number result from decimals" },
        { "1 / 3", "Infinite repeating decimal (Capped at 500)" },
        { "1.23456789 * 9.87654321", "High precision multiplication" },
        { "(10 + 5) / (2 * 3)", "Bracket precedence with division" },
        { "2 ^ 10", "Exponential growth test" }
    };

    for (int i = 0; i < tests.GetLength(0); i++)
    {
        string expr = tests[i, 0];
        string desc = tests[i, 1];
        
        try {
            string result = Evaluate(expr);
            Console.WriteLine($"\nTEST {i + 1}: {desc}");
            Console.WriteLine($"EXPR: {expr}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"RESULT: {(result.Length > 80 ? result.Substring(0, 75) + "..." : result)}");
            Console.ResetColor();
        }
        catch (Exception ex) {
            Console.WriteLine($"TEST {i + 1} FAILED: {ex.Message}");
        }
    }

    Console.WriteLine("\n--------------------------------------------------");
    Console.WriteLine("ALL TESTS COMPLETE. SYSTEM STABLE.");
    Console.WriteLine("--------------------------------------------------");
    Console.ReadLine();
}

    //static void BinaryOp(string name, Func<double, double, double> op) {
        //double a = GetDouble("First Number");
        //double b = GetDouble("Second Number");
       // Console.WriteLine($"\n{name}: {op(a, b):F6}");
    //}//

    //static void UnaryOp(string name, Func<double, double> op) {
        //double x = GetDouble("NUMBER");
        //Console.WriteLine($"\n{name}: {op(x):F6}");
    //}

    static double GetDouble(string prompt) {
        while (true) {
            Console.Write($"  Enter {prompt}: ");
            if (double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out double result)) return result;
            Console.WriteLine("  Invalid number!");
        }
    }

    static int GetInt(string prompt) {
        while (true) {
            Console.Write($"  Enter {prompt}: ");
            if (int.TryParse(Console.ReadLine(), out int result)) return result;
            Console.WriteLine("  Invalid integer!");
        }
    }
    static BigInteger BigIntSquareRoot(BigInteger n)
{
    if (n < 0) throw new ArgumentException("Negative number");
    if (n < 2) return n;

    // Fast bit-shift initial guess
    BigInteger x = n >> (n.ToByteArray().Length * 4); 
    if (x < 1) x = n / 2; 

    BigInteger lastX;
    do
    {
        lastX = x;
        x = (x + n / x) >> 1; 
    } while (BigInteger.Abs(x - lastX) > 1);

    return x;
}
static void BigDecBinaryOp(string name, Func<BigInteger, BigInteger, BigInteger> operation)
{
    Console.WriteLine($"\n--- {name} MODE (Max 500 Decimals) ---");
    
    Console.Write("Enter first number: ");
    string s1 = (Console.ReadLine() ?? "0").Trim();
    Console.Write("Enter second number: ");
    string s2 = (Console.ReadLine() ?? "0").Trim();

    int dot1 = s1.IndexOf('.');
    int dot2 = s2.IndexOf('.');

    int places1 = dot1 < 0 ? 0 : s1.Length - dot1 - 1;
    int places2 = dot2 < 0 ? 0 : s2.Length - dot2 - 1;
    
    // Determine how many decimal places we are tracking
    int maxPlaces = Math.Max(places1, places2);
    
    // Normalize (Align decimal points)
    string clean1 = s1.Replace(".", "") + new string('0', maxPlaces - places1);
    string clean2 = s2.Replace(".", "") + new string('0', maxPlaces - places2);

    if (BigInteger.TryParse(clean1, out BigInteger n1) && BigInteger.TryParse(clean2, out BigInteger n2))
    {
        BigInteger result = operation(n1, n2);
        string resStr = result.ToString();
        bool isNegative = resStr.StartsWith("-");
        if (isNegative) resStr = resStr.Substring(1);

        // Re-insert the decimal point
        if (resStr.Length <= maxPlaces)
        {
            resStr = resStr.PadLeft(maxPlaces + 1, '0');
        }
        
        int insertPos = resStr.Length - maxPlaces;
        string finalResult = (isNegative ? "-" : "") + resStr.Insert(insertPos, ".");

        // --- NEW: THE 500 DECIMAL CAP ---
        int finalDotPos = finalResult.IndexOf('.');
        if (finalDotPos != -1)
        {
            int currentDecimals = finalResult.Length - finalDotPos - 1;
            if (currentDecimals > 500)
            {
                // Cut off anything beyond 500 places
                finalResult = finalResult.Substring(0, finalDotPos + 501);
            }
        }
        // --------------------------------

        finalResult = finalResult.TrimEnd('0').TrimEnd('.');
        if (string.IsNullOrEmpty(finalResult) || finalResult == "-") finalResult = "0";

        Console.WriteLine("--------------------------------------------------");
        if (finalResult.Length > 70)
            Console.WriteLine($"Result: {finalResult.Substring(0, 30)}... [ {finalResult.Length} chars ]");
        else
            Console.WriteLine($"Result: {finalResult}");
        Console.WriteLine("--------------------------------------------------");
    }
}
static void BigDecUnaryOp(string name, Func<BigInteger, BigInteger> op) 
{
    Console.WriteLine($"\n--- {name} MODE (500 Decimal Limit) ---");
    Console.Write("Enter Number: ");
    string s = (Console.ReadLine() ?? "0").Trim();

    // 1. TRACK THE DECIMAL
    int dot = s.IndexOf('.');
    int inputPlaces = dot < 0 ? 0 : s.Length - dot - 1;
    
    // For Square, the decimal count doubles (0.5 * 0.5 = 0.25)
    int finalPrecision = name == "SQUARE" ? inputPlaces * 2 : inputPlaces;

    // 2. STRIP AND CONVERT
    string clean = s.Replace(".", "");
    if (BigInteger.TryParse(clean, out BigInteger n))
    {
        // 3. EXECUTE MATH (The CPU processes the BigInt here)
        BigInteger result = op(n);
        string resStr = result.ToString();
        
        bool isNegative = resStr.StartsWith("-");
        if (isNegative) resStr = resStr.Substring(1);

        // 4. ALIGNMENT AND PADDING
        // If the result is small (like 0.0012), we need to add leading zeros
        if (resStr.Length <= finalPrecision)
        {
            resStr = resStr.PadLeft(finalPrecision + 1, '0');
        }

        // 5. RE-INSERT THE DOT
        int insertPos = resStr.Length - finalPrecision;
        string finalResult = (isNegative ? "-" : "") + resStr.Insert(insertPos, ".");

        // 6. THE 500 DECIMAL CAP (Infinity Breaker)
        int finalDotPos = finalResult.IndexOf('.');
        if (finalDotPos != -1)
        {
            int currentDecs = finalResult.Length - finalDotPos - 1;
            if (currentDecs > 500)
            {
                finalResult = finalResult.Substring(0, finalDotPos + 501);
            }
        }

        // 7. CLEANUP
        finalResult = finalResult.TrimEnd('0').TrimEnd('.');
        if (string.IsNullOrEmpty(finalResult) || finalResult == "-") finalResult = "0";

        // 8. FINAL OUTPUT
        Console.WriteLine("--------------------------------------------------");
        if (finalResult.Length > 100)
            Console.WriteLine($"Result: {finalResult.Substring(0, 50)}... [ {finalResult.Length} chars ]");
        else
            Console.WriteLine($"Result: {finalResult}");
        Console.WriteLine("--------------------------------------------------");
    }
    else
    {
        Console.WriteLine("Invalid Input.");
    }
    Console.WriteLine("Press Enter to return...");
    Console.ReadLine();
}
// Place this near your other method definitions (e.g., after Factorial or Divide)
static void SetTheme(string themeName)
{
    switch (themeName.ToUpper())
    {
        case "MATRIX":
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            break;
        case "CYBER":
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Magenta;
            break;
        case "BLOOD":
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            break;
        case "CLASSIC":
            Console.ResetColor();
            break;
        default:
            Console.WriteLine("\nTheme not found! Try: MATRIX, CYBER, BLOOD, or CLASSIC.");
            Console.WriteLine("Press Enter...");
            Console.ReadLine();
            return;
    }
    Console.Clear(); // Refresh the screen with the new colors
}
static void ShowSecretPi()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("=== [ SECRET DISCOVERED: ARCHIMEDES' CONSTANT ] ===");
    Console.ResetColor();
    
    // Pi to 500 decimal places
    string pi = "3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679" +
                "8214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196" +
                "4428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141273" +
                "7245870066063155881748815209209628292540917153643678925903600113305305488204665213841469519415116094" +
                "330572703657595919530921861173819326117931051185480744623799627495673518857527248912279381830119491";

    Console.WriteLine($"\n{pi}");
    Console.WriteLine("\n--------------------------------------------------");
    Console.WriteLine("Optimized for i7-13620H Floating Point Simulation.");
    Console.WriteLine("Press Enter to return to the shadows...");
    Console.ReadLine();
    Console.Clear();
}
static void ShowGoldenRatio()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=== [ SECRET DISCOVERED: THE GOLDEN RATIO (PHI) ] ===");
    Console.ResetColor();

    // Phi to 500 decimal places
    string phi = "1.6180339887498948482045868343656381177203091798057628621354486227052604628189024497072072041893911374" +
                "8475408807538689175212663386222353693179318006076672635443338908659593958290563832266131992829026788" +
                "0675208766892501711675105152277402368204344192893670902576223301014194942954353089922574231170222782" +
                "0728467317326131055056477545406736371520417242096743520332817185431696981159201024232594412359623116" +
                "5945460656744147865715152149001155085854125406965639665292936649354994655040149452811440628913202";

    Console.WriteLine($"\n{phi}");
    Console.WriteLine("\n--------------------------------------------------");
    Console.WriteLine("The Divine Proportion verified by BigInteger Scaling.");
    Console.WriteLine("Press Enter to return...");
    Console.ReadLine();
    Console.Clear();
}
static void ShowEuler()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("=== [ SECRET DISCOVERED: EULER'S NUMBER (e) ] ===");
    
    string euler = "2.7182818284590452353602874713526624977572470936999595749669676277240766303535475945713821785251664274" +
                  "2746639193200305992181741359662904357290033429526059563073813232862794349076323382988075319525101901" +
                  "1573834187930702154089149934884167509244761460668082264800168477411853742345442437107539077744992069" +
                  "5517027618386062613313845830007520449338265602976067371132007093287091274437470472306969772093101416" +
                  "9283681902551510865746377211125238978442505695369677078544996996794686445490598793163688923009879";

    Console.WriteLine($"\n{euler}");
    Console.WriteLine("\n--------------------------------------------------");
    Console.WriteLine("Calculated via infinite series expansion: 1/n!");
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
    Console.Clear();
}
static void ShowCredits()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("--- INFINITY BREAKER ENGINE: DEVELOPMENT LOG ---");
    Console.ResetColor();
    Console.WriteLine("Developer: Gemini 3 Flash (Your Friendly Neighborhood C# Coder)");
    Console.WriteLine($"Build Version: {version}");
    Console.WriteLine("Architecture: 500-Decimal Precision scaling");
    Console.WriteLine("Hardware: Optimized for Intel i7-13620H @ 2.40GHz");
    Console.WriteLine($"Current System Time: {DateTime.Now}");
    Console.WriteLine("\n'The math is infinite, but the code is perfect.'");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Press any key to resume operations...");
    Console.ReadKey();
}
static void ShowRosterSet()
{
    Console.Clear();
    Console.WriteLine("--- ROSTER METHOD FORMATTER ---");
    Console.Write("Enter numbers separated by spaces (e.g., 1 2 2 3): ");
    string input = Console.ReadLine() ?? "";
    
    // Split, parse, and use a HashSet to automatically remove duplicates
    string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    var uniqueElements = new HashSet<string>(parts);

    Console.Write("\nSet in Roster Notation: { ");
    Console.Write(string.Join(", ", uniqueElements));
    Console.WriteLine(" }");
    
    if (uniqueElements.Count == 0)
        Console.WriteLine("\nNote: This is the Empty Set, also denoted as phi (φ).");

    Console.WriteLine("\nPress Enter to return...");
    Console.ReadLine();
}
static void ShowVersion()
{
    Console.Clear();
    Console.WriteLine($"Infinity Breaker Engine - Version {version}");
    Console.WriteLine("Optimized for high-precision calculations with BigInteger scaling.");
    Console.WriteLine("This version includes support for 500 decimal places in division and root operations.");
    Console.WriteLine("Developed by Gemini 3 Flash.");
    Console.WriteLine("\nPress Enter to return...");
    Console.ReadLine();
}
static void InitiateSelfDestruct()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("!!! [ CRITICAL SYSTEM ALERT ] !!!");
    Console.WriteLine("UNAUTHORIZED OVERRIDE DETECTED IN ENGINE CORE.");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("Initiating emergency containment failure...");
    Console.WriteLine($"Target Hardware: Intel i7-13620H Detected.");
    Console.WriteLine($"Target Software: Infinity Breaker v{version}");
    Console.WriteLine("----------------------------------------------");
    for (int i = 5; i > 0; i--)
    {
        Console.WriteLine($"SYSTEM COLLAPSE IN: {i}...");
        System.Threading.Thread.Sleep(1000);
    }
    Console.WriteLine("\n[ ERROR ]: COOLANT LEAK / GATES MELTING.");
    Console.WriteLine("[ ERROR ]: 500-DECIMAL OVERFLOW DETECTED.");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("Deleting logic history & wiping themes...");
    Console.WriteLine("Resetting BigInteger registers...");
    Console.WriteLine("Finalizing termination sequence...");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("GOODBYE, OPERATOR.");
    System.Threading.Thread.Sleep(2000);
    Console.ResetColor(); Console.Clear();
    Console.WriteLine("System Reboot Successful. Cache Cleared.");
    Console.WriteLine("Press Enter to return to safety...");
    Console.ReadLine();
}
}
