// Arithmetic operations Part
// Converting non-negative decimal numbers to binary
let convertToBinary (num: int) : int list =
    let rec toBinary num =
        if num = 0 then [0] // Handle case when num is 0
        else if num < 0 then toBinary (-num) // Handle negative numbers
        else (num % 2) :: toBinary (num / 2)
    let binaryList = List.rev (toBinary num)
    List.replicate (8 - List.length binaryList) 0 @ binaryList


// Converting non-negative binary numbers to decimal
let convertToDecimal (binaryList: int list) : int =
    let rec convertToDecimalHelper list carry =
        match list with
        | [] -> carry
        | head :: tail ->
            let newCarry = carry * 2 + head
            convertToDecimalHelper tail newCarry
    convertToDecimalHelper binaryList 0 


// Addition:
let addBinary (binaryList1: int list) (binaryList2: int list) : int list =
    let rec addHelper list1 list2 carry result =
        match list1, list2 with
        | [], [] -> if carry = 1 then result else result 
        | [], _ | _, [] -> failwith "Input lists must have the same length"
        | bit1 :: tail1, bit2 :: tail2 ->
            let sum = bit1 + bit2 + carry
            let newBit = sum % 2
            let newCarry = if sum > 1 then 1 else 0
            addHelper tail1 tail2 newCarry (newBit :: result)
    let result = addHelper (List.rev binaryList1) (List.rev binaryList2) 0 []
    List.rev result 

let binaryToString (binaryList: int list) (reverse: bool) : string =
    let processedList = if reverse then List.rev binaryList else binaryList
    processedList
    |> List.map string
    |> String.concat ""


// Converting negative decimal numbers to binary:
// Get absolute number
let absoluteValue (num: int) : int =
    if num < 0 then -num else num
// Invert all the values (NOT operation)
let NOT (binaryList: int list) : int list =
    List.map (fun bit -> if bit = 0 then 1 else 0) binaryList
// Add one
let addOne (binaryList: int list) : int list =
    let rec addOneHelper list carry result =
        match list with
        | [] -> if carry = 1 then [1] else result
        | bit :: tail ->
            let sum = bit + carry
            let newBit = sum % 2
            let newCarry = if sum > 1 then 1 else 0
            addOneHelper tail newCarry (newBit :: result)
    addOneHelper (List.rev binaryList) 1 []
// Check the Sum is zero
let checkSumToZero (num1: int) (num2: int) : bool =
    let binaryList1 = convertToBinary num1
    let binaryList2 = convertToBinary num2
    let sumResult = addBinary binaryList1 binaryList2
    List.forall (fun bit -> bit = 0) sumResult


// Logical operations Part
let convertHexToBinary (hex: int) : int list =
    let binaryString = System.Convert.ToString(hex, 2).PadLeft(8, '0')
    binaryString |> Seq.map (fun c -> int(c.ToString())) |> List.ofSeq
// AND:
let bitwiseAND (binaryList1: int list) (binaryList2: int list) : int list =
    List.map2 (fun bit1 bit2 -> if bit1 = 1 && bit2 = 1 then 1 else 0) binaryList1 binaryList2
// OR:
let bitwiseOR (binaryList1: int list) (binaryList2: int list) : int list =
    List.map2 (fun bit1 bit2 -> if bit1 = 1 || bit2 = 1 then 1 else 0) binaryList1 binaryList2
// XOR:
let bitwiseXOR (binaryList1: int list) (binaryList2: int list) : int list =
    List.map2 (fun bit1 bit2 -> if bit1 = bit2 then 0 else 1) binaryList1 binaryList2
// NOT:
let bitwiseNOT (binaryList: int list) : int list =
    List.map (fun bit -> if bit = 0 then 1 else 0) binaryList
//
//
//
// Test Arithmetic Operations Function
// DecimalToBinary
let testDecimalToBinary num =
    let binaryNumber = convertToBinary num
    printfn "Arithmetic operations Part" 
    printfn "*********************"
    printfn "*********************"
    printfn "*********************"
    printfn "Converting non-negative decimal numbers to binary"
    printfn "The binary representation of %d is: %A" num binaryNumber
    printfn ""
// BinaryToDecimal
let testBinaryToDecimal binaryList =
    let decimalNumber = convertToDecimal binaryList
    printfn "Converting non-negative binary numbers to decimal"
    printfn "The decimal representation of %A is: %d" binaryList decimalNumber
    printfn ""
// NegativeDecimalToBinary
let testNegativeDecimalToBinary num =
    let absValue = absoluteValue num
    let binary = convertToBinary absValue 
    let complementBinary = NOT binary 
    let addOneResult = addOne complementBinary 
    let positiveBinary = convertToBinary absValue 
    let sumResult = addBinary positiveBinary addOneResult 
    let checkZeroSum = List.forall (fun bit -> bit = 0) sumResult 
    printfn "Converting negative decimal numbers to binary and check if sum with its positive is zero:"
    printfn "%d decimal to binary:" num
    printfn "1. %A" binary
    printfn "2. NOT -> %A" complementBinary
    printfn "3. ADD 1 -> %A" addOneResult
    printfn "4. Check %d + (%d) = 0? %A" num (-num) checkZeroSum
    printfn ""
let testBinaryAddition num1 num2 =
    let binaryList1 = convertToBinary (absoluteValue num1)
    let binaryList2 = convertToBinary (absoluteValue num2)
    let resultBinaryList =
        if num1 < 0 && num2 < 0 then
            let complementBinary1 = addOne (NOT binaryList1)
            let complementBinary2 = addOne (NOT binaryList2)
            let result = addBinary complementBinary1 complementBinary2
            let truncatedResult = if List.length result > 8 then List.skip (List.length result - 8) result else result
            printfn "The Addition of %d and %d is:" num1 num2
            printfn "%d -> %s" (absoluteValue num1) (binaryToString binaryList1 false)
            printfn "NOT -> %s" (binaryToString (NOT binaryList1) false)
            printfn "ADD 1 -> %s" (binaryToString complementBinary1 false)
            printfn "-%d -> %s" (absoluteValue num1) (binaryToString complementBinary1 false)
            printfn "%d -> %s" (absoluteValue num2) (binaryToString binaryList2 false)
            printfn "NOT -> %s" (binaryToString (NOT binaryList2) false)
            printfn "ADD 1 -> %s" (binaryToString complementBinary2 false)
            printfn "-%d -> %s" (absoluteValue num2) (binaryToString complementBinary2 false)
            printfn "********************"
            printfn "%d -> %s" (num1 + num2) (binaryToString truncatedResult true)
        elif num1 < 0 || num2 < 0 then
            let positiveNum = if num1 > 0 then num1 else num2
            let negativeNum = if num1 < 0 then num1 else num2
            let positiveBinary = if num1 > 0 then binaryList1 else binaryList2
            let negativeBinary = NOT (if num1 < 0 then binaryList1 else binaryList2)
            let addOneResult = addOne negativeBinary
            let result = addBinary positiveBinary addOneResult
            let truncatedResult = if List.length result > 8 then List.skip (List.length result - 8) result else result
            printfn "The Addition of %d and %d is:" num1 num2
            printfn "%d -> %s" positiveNum (binaryToString positiveBinary false)
            printfn "%d -> %s" (absoluteValue negativeNum) (binaryToString (convertToBinary (absoluteValue negativeNum)) false)
            printfn "NOT -> %s" (binaryToString negativeBinary false)
            printfn "ADD 1 -> %s" (binaryToString addOneResult false)
            printfn "-%d -> %s" (absoluteValue negativeNum) (binaryToString addOneResult true)
            printfn "********************"
            printfn "%d -> %s" (num1 + num2) (binaryToString truncatedResult true)
        else
            let result = addBinary binaryList1 binaryList2
            let truncatedResult = if List.length result > 8 then List.skip (List.length result - 8) result else result
            printfn "The Addition of %d and %d is:" num1 num2
            printfn "%d -> %s" num1 (binaryToString binaryList1 false)
            printfn "%d -> %s" num2 (binaryToString binaryList2 false)
            printfn "********************"
            printfn "%d -> %s" (num1 + num2) (binaryToString truncatedResult true)
    printfn ""







// BinarySubtraction 
let testBinarySubtraction num1 num2 =
    let binaryList2 = convertToBinary num2
    let notResult = NOT binaryList2
    let addOneResult = addOne notResult
    let binaryList1 = convertToBinary num1
    let result = addBinary binaryList1 addOneResult
    let finalResult = if List.length result > 8 then List.skip (List.length result - 8) result else result
    printfn "Subtraction:"
    printfn "The Subtraction of %d and %d is:" num1 num2
    printfn "1. %d -> %s" num2 (binaryToString binaryList2 false) 
    printfn "2. NOT -> %s" (binaryToString notResult false) 
    printfn "3. ADD 1 -> %s" (binaryToString addOneResult false) 
    printfn "*********************"
    printfn "4. %d -> %s" num1 (binaryToString binaryList1 false) 
    printfn "5. -%d -> %s" num2 (binaryToString addOneResult true) 
    printfn "*********************"
    printfn "6. %d -> %s" (num1 - num2) (binaryToString finalResult true) 
    printfn ""
//
//
//

//
//
//
// Test Logical Operations Functions
// AND
let testLogicalOperationsAND num1 num2 =
    let binaryList1 = convertHexToBinary num1
    let binaryList2 = convertHexToBinary num2
    printfn "AND:"
    printfn "1. %X -> %s" num1 (binaryToString binaryList1 false)
    printfn "2. %X -> %s" num2 (binaryToString binaryList2 false)
    printfn "3. ---------------------------"
    let result = bitwiseAND binaryList1 binaryList2
    printfn "4. %X -> %s" (convertToDecimal result) (binaryToString result true)
    printfn ""
// OR
let testLogicalOperationsOR num1 num2 =
    let binaryList1 = convertHexToBinary num1
    let binaryList2 = convertHexToBinary num2
    printfn "OR:"
    printfn "1. %X -> %s" num1 (binaryToString binaryList1 false)
    printfn "2. %X -> %s" num2 (binaryToString binaryList2 false)
    printfn "3. ---------------------------"
    let result = bitwiseOR binaryList1 binaryList2
    printfn "4. %X -> %s" (convertToDecimal result) (binaryToString result false)
    printfn ""
// XOR
let testLogicalOperationsXOR num1 num2 =
    let binaryList1 = convertHexToBinary num1
    let binaryList2 = convertHexToBinary num2
    printfn "XOR:"
    printfn "1. %X -> %s" num1 (binaryToString binaryList1 false)
    printfn "2. %X -> %s" num2 (binaryToString binaryList2 false)
    printfn "3. ---------------------------"
    let result = bitwiseXOR binaryList1 binaryList2
    printfn "4. %X -> %s" (convertToDecimal result) (binaryToString result false)
    printfn ""
// NOT
let testLogicalOperationsNOT num =
    let binaryList = convertHexToBinary num
    printfn "NOT:"
    printfn "1. %X -> %s" num (binaryToString binaryList false)
    printfn "2. ---------------------------"
    let result = bitwiseNOT binaryList
    printfn "3. %X -> %s" (convertToDecimal result) (binaryToString result false)
    printfn ""



// Run Arithmetic Operations Test Part
testDecimalToBinary 76 
testBinaryToDecimal [0; 0; 1; 1; 0; 0; 0; 0] 
testNegativeDecimalToBinary -83
testBinaryAddition -7 -83
//testBinarySubtraction 45 -6
// Run Logical Operations Test Part
//testLogicalOperationsAND 0x48 0x84
//testLogicalOperationsOR 0x48 0x84
//testLogicalOperationsXOR 0xA5 0xF1
//testLogicalOperationsNOT 0xF1
