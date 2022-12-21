﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            // starting sympol
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        // ================================================================================================================
        // Implement your production rules here.
        /*
            RULES :
            - start with starting symbol.
            - create node function for each non terminal [Procedure].
            - for each procedure implement its R.H.S.
            - if you find terminal with small letter [Token] then add match function. 
            - else if you find NON terminal with capital lette [Procedure] then add this function.
            - in case of the function contains or then check for prefix of each segement using if statement.
                    if (TokenStream[InputPointer].token_type == Token_Class.)
            - in case of epslon return null.

            RULES for clean code : 
            - each node start with capital letter. 
                Node Program(){}
            - the created object inside node start with small letter and the passed name is the same as node function name.
                Node program = new Node("Program");
        */

        Node Program()
        {
            // completed
            // Program → Function_statements Main_function
            Node program = new Node("Program");
            program.Children.Add(Function_statements());
            program.Children.Add(Main_function());
            return program;
        }

        Node Function_statements()
        {
            // completed
            // Function_statements → Function_statement Function_statementsDash
            Node function_statements = new Node("Function_statements");
            function_statements.Children.Add(Function_statement());
            function_statements.Children.Add(Function_statements_Dash());
            return function_statements;
        }



        Node Function_statement()
        {
            // completed
            // Function_statement → Function_declaration Function_body | 𝜀
            Node function_statement = new Node("Function_statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                function_statement.Children.Add(Function_declaration());
                function_statement.Children.Add(Function_body());
                return function_statement;
            }
            else
            {
                return null;
            }

        }

        Node Function_statements_Dash()
        {
            // completed
            // Function_statements_Dash → Function_statement Function_statements’ | 𝜀
            Node function_statements_Dash = new Node("Function_statements_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                function_statements_Dash.Children.Add(Function_statement());
                function_statements_Dash.Children.Add(Function_statements_Dash());
                return function_statements_Dash;
            }
            return null;
        }

        Node Function_declaration()
        {
            // completed
            // Function _declaration → Datatype identifier (Parameters)
            Node function_declaration = new Node("Function_declaration");
            function_declaration.Children.Add(Datatype());
            function_declaration.Children.Add(match(Token_Class.Idenifier));
            function_declaration.Children.Add(match(Token_Class.LParanthesis));
            // function_declaration.Children.Add(Parameters());
            function_declaration.Children.Add(match(Token_Class.RParanthesis));
            return function_declaration;
        }

        Node Function_body()
        {
            // completed
            // Function _body → { Statements Return_statement }
            Node function_body = new Node("Function_body");
            function_body.Children.Add(match(Token_Class.LPracket));
            function_body.Children.Add(Statements());
            function_body.Children.Add(Return_statement());
            function_body.Children.Add(match(Token_Class.RPracket));
            return function_body;
        }



  



        Node Main_function()
        {
            // completed
            // Main_function → Datatype main() Function_body
            Node main_function = new Node("Main_function");
            main_function.Children.Add(Datatype());
            main_function.Children.Add(match(Token_Class.Main));
            main_function.Children.Add(match(Token_Class.LParanthesis));
            main_function.Children.Add(match(Token_Class.RParanthesis));
            main_function.Children.Add(Function_body());
            return main_function;
        }

        Node Datatype()
        {
            // Completed
            // Datatype → int | float | string
            Node datatype = new Node("Datatype");
            if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                datatype.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return datatype;
        }



        Node Statements()
        {
            // completed
            // Statements → Statement Statements’
            Node statements = new Node("Statements");
            statements.Children.Add(Statement());
            statements.Children.Add(StatementsDash());
            return statements;
        }

        Node Statement()
        {
            // Statement → Write_statement | Read_statement | Assignment_statement | Declaration_statement | If_statement |Repeat_statement | Function_call
            Node statement = new Node("Statement");

            return statement;
        }

        Node StatementsDash()
        {
            // Statements’ → Statement Statements’ | 𝜀
            Node statementsDash = new Node("StatementsDash");

            return statementsDash;
        }

        Node Return_statement()
        {
            // completed    
            // Return_statement → return Expression ;
            Node return_statement = new Node("Return_statement");
            return_statement.Children.Add(match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(match(Token_Class.Semicolon));
            return return_statement;
        }

        Node Ret_statement()
        {
            // completed
            // Ret_statement → Return_statement | 𝜀
            Node ret_statement = new Node("Ret_statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                ret_statement.Children.Add(Return_statement());
                return ret_statement;
            }
            else
            {
                return null;
            }
        }
        Node Write_statement()
        {
            // completed    
            // Write_statement → write Write_statement_Dash
            Node write_statement = new Node("Write_statement");
            write_statement.Children.Add(match(Token_Class.Write));
            write_statement.Children.Add(Write_statement_Dash());
            return write_statement;
        }

        Node Read_statement()
        {
            // completed
            // Read_statement → read identifier ;
            Node read_statement = new Node("Read_statement");
            read_statement.Children.Add(match(Token_Class.Read));
            read_statement.Children.Add(match(Token_Class.Idenifier));
            return read_statement;
        }

        Node Write_statement_Dash()
        {
            // completed
            // Write_statement’ → Expression ; | endl ;
            Node write_statement_dash = new Node("Write_statement_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.String)
            {
                write_statement_dash.Children.Add(Expression());
                write_statement_dash.Children.Add(match(Token_Class.Semicolon));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                write_statement_dash.Children.Add(match(Token_Class.Endl));
                write_statement_dash.Children.Add(match(Token_Class.Semicolon));
            }
            return write_statement_dash;
        }

        Node Expression()
        {
            // Expression → stringLine | Term | Equation
            Node expression = new Node("Expression");
            

            return expression;
        }

        Node Term()
        {
            // completed
            // Term → number | identifier | Function_call
            Node term = new Node("Term");
            if (TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && true/*check for i*/ && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
            {
                term.Children.Add(Function_call());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                term.Children.Add(match(Token_Class.Idenifier));
            }
            return term;
        }

        Node Function_call()
        {
            // completed
            // Function_call → identifier (Identifier_list | 𝜀) 
            Node function_call = new Node("Function_call");
            function_call.Children.Add(match(Token_Class.Idenifier));
            function_call.Children.Add(match(Token_Class.LParanthesis));
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                function_call.Children.Add(match(Token_Class.Idenifier));
            }
            else
            {
                function_call.Children.Add(match(Token_Class.RParanthesis));
                return function_call;
            }
            function_call.Children.Add(match(Token_Class.RParanthesis));
            return function_call;
        }

        Node Identifier_list()
        {
            // completed
            // Identifier_list → Id Identifier_list_Dash
            Node identifier_list = new Node("Identifier_list");
            identifier_list.Children.Add(Id());
            identifier_list.Children.Add(Identifier_list_Dash());
            return identifier_list;
        }

        Node Identifier_list_Dash()
        {
            // completed
            // Identifier_list’ → , Id Identifier_list’ | eplson
            Node identifier_list_Dash = new Node("Identifier_list_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                identifier_list_Dash.Children.Add(match(Token_Class.Comma));
                identifier_list_Dash.Children.Add(Id());
                identifier_list_Dash.Children.Add(Identifier_list_Dash());
                return identifier_list_Dash;
            }
            else
            {
                return null;
            }
        }

        Node ConditionOp()
        {
            // completed
            // ConditionOp → notEqualOp | equalOp | lessThanOp |greaterThanOp
            Node conditionOp = new Node("ConditionOp");
            if (TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.NotEqualOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.ConditionEqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.ConditionEqualOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.LessThanOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.GreaterThanOp));
            }
            return conditionOp;
        }

        Node BooleanOp()
        {
            // completed
            //  BooleanOp → andOp | orOp
            Node booleanOp = new Node("BooleanOp");
            if (TokenStream[InputPointer].token_type == Token_Class.And || TokenStream[InputPointer].token_type == Token_Class.Or)
            {
                booleanOp.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return booleanOp;
        }

        Node AddOp()
        {
            // completed
            // AddOp → plusOp |minusOp
            Node addOp = new Node("AddOp");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp)
            {
                addOp.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return addOp;
        }

        Node MultOp()
        {
            // completed
            // MultOp → multiplyOp | devideOp
            Node multOp = new Node("MultOp");
            if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                multOp.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return multOp;
        }

        Node Condition()
        {
            // completed
            // Condition → identifier ConditionOp Term
            Node condition = new Node("Condition");
            condition.Children.Add(match(Token_Class.Idenifier));
            condition.Children.Add(ConditionOp());
            condition.Children.Add(Term());
            return condition;
        }

        Node Condition_statement()
        {
            // completed
            // Condition_statement → Condition Condition_statement_Dash
            Node condition_statement = new Node("Condition_statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Condition_statement_Dash());
            return condition_statement;
        }

        Node Condition_statement_Dash()
        {
            // completed
            // Condition_statement’ → BooleanOp Condition Condition_statement’| 𝜀
            Node condition_statement_Dash = new Node("Condition_statement_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.And || TokenStream[InputPointer].token_type == Token_Class.Or)
            {
                condition_statement_Dash.Children.Add(BooleanOp());
                condition_statement_Dash.Children.Add(Condition());
                condition_statement_Dash.Children.Add(Condition_statement_Dash());
                return condition_statement_Dash;
            }
            return null;
        }

        Node Id()
        {
            // completed
            // Id → identifier Id’
            Node id = new Node("Id");
            id.Children.Add(match(Token_Class.Idenifier));
            id.Children.Add(Id_Dash());
            return id;
        }

        Node Id_Dash()
        {
            // completed
            // Id_Dash → 𝜀 | assignmentOp Expression
            Node id_Dash = new Node("Id_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.AssignmentOp)
            {
                id_Dash.Children.Add(match(Token_Class.AssignmentOp));
                id_Dash.Children.Add(Expression());
                return id_Dash;
            }
            else
            {
                return null;
            }
        }




        // ================================================================================================================
        // match : deal with tokens 
        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());
                    return newNode;
                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        // ================================================================================================================
        // Print Parse Tree
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
