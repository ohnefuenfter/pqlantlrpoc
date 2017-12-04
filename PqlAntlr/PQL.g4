grammar PQL;

/*
 * Parser Rules
 * [Case.Id]
 * 'prefix[Case.Id]pos''tfix'
 * [Func([Case.Id])]
 * [Func('prefix[Case.Id]postfix')]
 * [Id|Case|CasePK=[RootPK]]
 * [Id|Case|ParentCase.Id='prefix[Case.Id]postfix']
 */

pqlExprList
	: pqlStmtList EOF		
	;

pqlStmtList : (pqlStmt | quotedPqlStmt)+ ;

pqlStmt : '[' pqlOp ']'
		  ;

pqlOp: identifier
	   | quotedPqlStmt
	   | funcCall
	   | hqlExpr
	   | sqlExpr
	   ;

quotedPqlStmt : '\'' stringWithDblQuote? pqlStmt stringWithDblQuote? '\''
				;

funcCall : identifier OPENBR paramterList CLOSEBR
		   ;

paramterList : parameter
			   | parameter ',' paramterList
			;

parameter : number
			| pqlStmt
			| stringLiteral
			;

hqlExpr : identifier PIPE identifier (alias)? PIPE testExprOrList PIPE separatorExpr
		  |identifier PIPE identifier (alias)? PIPE testExprOrList
		   ;

sqlExpr : S Q L ':' identifier PIPE identifier (alias)? PIPE testExprOrList PIPE separatorExpr 
		  | S Q L ':' identifier PIPE identifier (alias)? PIPE testExprOrList
		;

testExprOrList : testExpr
				 | testExpr O R testExprAndList
				    | testExpr O R testExprOrList
				 ;
testExprAndList : testExpr
				| testExpr A N D testExprAndList
				;

testExpr : identifier testOp testValue
		   | testValue testOp identifier
			 | testValue testOp testValue
			   ;

testValue : stringLiteral
			| pqlStmt
			| number
			| 'true'
			| 'false'
			| 'null'
			;
testOp : '>'
		 | '<'
		 | '<='
		 | '>='
		 | '='
		 | '<>'
		 | 'is'
		 ;

separatorExpr : stringLiteral
				| WORD
				  ;

number : DIGIT+ ;

identifier : id 
		| id DOT identifier
		;	
alias : id ;
id : ID ; 

stringWithDblQuote: ( ~'\'' | ~'[' | '\'\'' )* ;
stringLiteral
 : '\'' ( ~'\'' | ~'[' | '\'\'' )* '\''
 ;

/*
 * Lexer Rules
 */

OPENBR : '(' ;
CLOSEBR : ')' ;
ID : [a-zA-Z_] [a-zA-Z0-9_]* 
	;
fragment S : [s|S];
fragment Q : [q|Q];
fragment L : [l|L];
fragment O :[o|O];
fragment R :[r|R];
fragment A :[a|A];
fragment N :[n|N];
fragment D : [d|D];
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;

ACHAR : (LOWERCASE | UPPERCASE)+ ;
SCHAR
	: ('$' | ',' | '~' | '!' | '@' | '#' | '$' | '%' | '^' | '&' | '*' | '_' | '+' | '-' | ':' | ';' |  '?' | '/' | '\\' | '{' | '}')
	;
PIPE : '|' ;
DOT	: '.'	;
DIGIT : [0-9]  ;
WS
   : ('\r' | '\n' |' '|'\t')+ -> skip
   ;
WORD : (ACHAR | SCHAR | DIGIT)+ ; 


