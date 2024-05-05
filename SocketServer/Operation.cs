public class Operation : OperationBase
{
    public string Operator { get; set; }
    public double FirtsValue { get; set; }
    public double SecondValue { get; set; }

    public override double Execute()
    {
        switch (Operator) {
            case "+":
                return Addition();
            case "-":
                return Subtraction();
            case "*":
                return Multiplication();
            case "/":
                return Division();
            default:
                return 0;
        }
    }
    public override double Addition()
    {
        return FirtsValue + SecondValue;
    }

    public override double Division()
    {
        return FirtsValue / SecondValue;
    }

    public override double Multiplication()
    {
        return FirtsValue * SecondValue;
    }

    public override double Subtraction()
    {
        return FirtsValue - SecondValue;
    }
}