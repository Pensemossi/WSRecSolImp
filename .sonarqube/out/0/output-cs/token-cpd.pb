Ï
OC:\Users\Administrador\source\repos\WSInicial\WSInicial\Capa Negocio\Permiso.cs
	namespace 	
	WSInicial
 
. 
Capa_Negocio  
{ 
public 

class 

ClsPermiso 
{		 
private

 
string

 
strNombreDocumento

 )
;

) *
private 
string 
strTipoDocumento '
;' (
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
NombreDocumento %
{& '
get( +
=>, .
strNombreDocumento/ A
;A B
setC F
=>G I
strNombreDocumentoJ \
=] ^
value_ d
;d e
}f g
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
TipoDocumento #
{$ %
get& )
=>* ,
strTipoDocumento- =
;= >
set? B
=>C E
strTipoDocumentoF V
=W X
valueY ^
;^ _
}` a
} 
public 

class 
ClsPermisos 
{ 
private 
List 
< 

ClsPermiso 
>  

lstPermiso! +
;+ ,
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
,s t
Typeu y
=z {
typeof	| Ç
(
Ç É

ClsPermiso
É ç
)
ç é
)
é è
]
è ê
public 
List 
< 

ClsPermiso 
> 
Permiso  '
{ 	
get 
{ 
return 

lstPermiso !
;! "
} 
set 
{   

lstPermiso!! 
=!! 
value!! "
;!!" #
}"" 
}## 	
}$$ 
}%% ß'
PC:\Users\Administrador\source\repos\WSInicial\WSInicial\Capa Negocio\Producto.cs
	namespace 	
	WSInicial
 
. 
Capa_Negocio  
{ 
public 

class 
ClsProducto 
{		 
private

 
string

 
strDesProducto

 %
;

% &
private 
string 
strCantidadProducto *
;* +
private 
string 
strPaisOrigen $
;$ %
private 
ClsPermisos 

lstPermiso &
;& '
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
DesProducto !
{" #
get$ '
=>( *
strDesProducto+ 9
;9 :
set; >
=>? A
strDesProductoB P
=Q R
valueS X
;X Y
}Z [
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
CantidadProducto &
{' (
get) ,
=>- /
strCantidadProducto0 C
;C D
setE H
=>I K
strCantidadProductoL _
=` a
valueb g
;g h
}i j
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 

PaisOrigen  
{! "
get# &
=>' )
strPaisOrigen* 7
;7 8
set9 <
=>= ?
strPaisOrigen@ M
=N O
valueP U
;U V
}W X
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
,s t
Typeu y
=z {
typeof	| Ç
(
Ç É
ClsPermisos
É é
)
é è
)
è ê
]
ê ë
public 
ClsPermisos 
Permisos #
{ 	
get 
{ 
return 

lstPermiso !
;! "
} 
set 
{   

lstPermiso!! 
=!! 
value!! "
;!!" #
}"" 
}## 	
}$$ 
public&& 

class&& 
ClsProductos&& 
{'' 
private(( 
List(( 
<(( 
ClsProducto((  
>((  !
lstProducto((" -
;((- .
[** 	
System**	 
.** 
Xml** 
.** 
Serialization** !
.**! "
XmlElementAttribute**" 5
(**5 6
Form**6 :
=**; <
System**= C
.**C D
Xml**D G
.**G H
Schema**H N
.**N O
XmlSchemaForm**O \
.**\ ]
Unqualified**] h
,**h i
Order**j o
=**p q
$num**r s
,**s t
Type**u y
=**z {
typeof	**| Ç
(
**Ç É
ClsProducto
**É é
)
**é è
)
**è ê
]
**ê ë
public++ 
List++ 
<++ 
ClsProducto++ 
>++  
Producto++! )
{,, 	
get-- 
{.. 
return// 
lstProducto// "
;//" #
}00 
set11 
{22 
lstProducto33 
=33 
value33 #
;33# $
}44 
}55 	
}66 
}77 ¯
YC:\Users\Administrador\source\repos\WSInicial\WSInicial\Capa Negocio\SolicitudEspecial.cs
	namespace 	
	WSInicial
 
. 
Capa_Negocio  
{ 
public 

class  
ClsSolicitudEspecial %
{		 
private

 
string

 &
strNombreSolicitudEspecial

 1
;

1 2
private 
string +
strDescripcionSolicitudEspecial 6
;6 7
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
NomSolEspecial $
{% &
get' *
=>+ -&
strNombreSolicitudEspecial. H
;H I
setJ M
=>N P&
strNombreSolicitudEspecialQ k
=l m
valuen s
;s t
}u v
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
DesSolEspecial $
{% &
get' *
=>+ -+
strDescripcionSolicitudEspecial. M
;M N
setO R
=>S U+
strDescripcionSolicitudEspecialV u
=v w
valuex }
;} ~
}	 Ä
} 
public 

class $
ClsSolicitudesEspeciales )
{ 
private 
List 
<  
ClsSolicitudEspecial )
>) *
	lstSolEsp+ 4
;4 5
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
,s t
Typeu y
=z {
typeof	| Ç
(
Ç É"
ClsSolicitudEspecial
É ó
)
ó ò
)
ò ô
]
ô ö
public 
List 
<  
ClsSolicitudEspecial (
>( )
SolicitudEspecial* ;
{ 	
get 
{ 
return 
	lstSolEsp  
;  !
} 
set 
{   
	lstSolEsp!! 
=!! 
value!! !
;!!! "
}"" 
}## 	
}$$ 
}%% ºl
dC:\Users\Administrador\source\repos\WSInicial\WSInicial\Capa Negocio\SolicitudLicenciaImportacion.cs
	namespace 	
	WSInicial
 
. 
Capa_Negocio  
{ 
public		 

class		 (
SolicitudLicenciaImportacion		 -
{

 
private 
string 
strReapertura $
;$ %
private 
string 
strNumTempSol $
;$ %
private 
string 
strTipoSolicitud '
;' (
private 
string 
strNitImportador '
;' (
private 
string 
strNombreImportador *
;* +
private 
string 
strClaseImportador )
;) *
private 
string 
strTipoCancelacion )
;) *
private 
string  
strMotivoCancelacion +
;+ ,
private 
string 
strVia 
; 
private 
string 

strOtraVia !
;! "
private 
string 
	strAduana  
;  !
private 
string 
strPuertoEmbarque (
;( )
private 
string !
strOtroPuertoEmbarque ,
;, -
private 
string 
strPaisCompra $
;$ %
private $
ClsSolicitudesEspeciales (
	lstSolEsp) 2
;2 3
private 
ClsSubPartidas 

lstSubPart )
;) *
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 

Reapertura  
{! "
get# &
=>' )
strReapertura* 7
;7 8
set9 <
=>= ?
strReapertura@ M
=N O
valueP U
;U V
}W X
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public   
string   
NumTemporalSol   $
{  % &
get  ' *
=>  + -
strNumTempSol  . ;
;  ; <
set  = @
=>  A C
strNumTempSol  D Q
=  R S
value  T Y
;  Y Z
}  [ \
["" 	
System""	 
."" 
Xml"" 
."" 
Serialization"" !
.""! "
XmlElementAttribute""" 5
(""5 6
Form""6 :
=""; <
System""= C
.""C D
Xml""D G
.""G H
Schema""H N
.""N O
XmlSchemaForm""O \
.""\ ]
Unqualified""] h
,""h i
Order""j o
=""p q
$num""r s
)""s t
]""t u
public## 
string## 
TipoSolicitud## #
{##$ %
get##& )
=>##* ,
strTipoSolicitud##- =
;##= >
set##? B
=>##C E
strTipoSolicitud##F V
=##W X
value##Y ^
;##^ _
}##` a
[%% 	
System%%	 
.%% 
Xml%% 
.%% 
Serialization%% !
.%%! "
XmlElementAttribute%%" 5
(%%5 6
Form%%6 :
=%%; <
System%%= C
.%%C D
Xml%%D G
.%%G H
Schema%%H N
.%%N O
XmlSchemaForm%%O \
.%%\ ]
Unqualified%%] h
,%%h i
Order%%j o
=%%p q
$num%%r s
)%%s t
]%%t u
public&& 
string&& 
NitImportador&& #
{&&$ %
get&&& )
=>&&* ,
strNitImportador&&- =
;&&= >
set&&? B
=>&&C E
strNitImportador&&F V
=&&W X
value&&Y ^
;&&^ _
}&&` a
[(( 	
System((	 
.(( 
Xml(( 
.(( 
Serialization(( !
.((! "
XmlElementAttribute((" 5
(((5 6
Form((6 :
=((; <
System((= C
.((C D
Xml((D G
.((G H
Schema((H N
.((N O
XmlSchemaForm((O \
.((\ ]
Unqualified((] h
,((h i
Order((j o
=((p q
$num((r s
)((s t
]((t u
public)) 
string)) 
NomImportador)) #
{))$ %
get))& )
=>))* ,
strNombreImportador))- @
;))@ A
set))B E
=>))F H
strNombreImportador))I \
=))] ^
value))_ d
;))d e
}))f g
[++ 	
System++	 
.++ 
Xml++ 
.++ 
Serialization++ !
.++! "
XmlElementAttribute++" 5
(++5 6
Form++6 :
=++; <
System++= C
.++C D
Xml++D G
.++G H
Schema++H N
.++N O
XmlSchemaForm++O \
.++\ ]
Unqualified++] h
,++h i
Order++j o
=++p q
$num++r s
)++s t
]++t u
public,, 
string,, 
ClaseImportador,, %
{,,& '
get,,( +
=>,,, .
strClaseImportador,,/ A
;,,A B
set,,C F
=>,,G I
strClaseImportador,,J \
=,,] ^
value,,_ d
;,,d e
},,f g
[.. 	
System..	 
... 
Xml.. 
... 
Serialization.. !
...! "
XmlElementAttribute.." 5
(..5 6
Form..6 :
=..; <
System..= C
...C D
Xml..D G
...G H
Schema..H N
...N O
XmlSchemaForm..O \
...\ ]
Unqualified..] h
,..h i
Order..j o
=..p q
$num..r s
)..s t
]..t u
public// 
string// 
TipoCancelacion// %
{//& '
get//( +
=>//, .
strTipoCancelacion/// A
;//A B
set//C F
=>//G I
strTipoCancelacion//J \
=//] ^
value//_ d
;//d e
}//f g
[11 	
System11	 
.11 
Xml11 
.11 
Serialization11 !
.11! "
XmlElementAttribute11" 5
(115 6
Form116 :
=11; <
System11= C
.11C D
Xml11D G
.11G H
Schema11H N
.11N O
XmlSchemaForm11O \
.11\ ]
Unqualified11] h
,11h i
Order11j o
=11p q
$num11r s
)11s t
]11t u
public22 
string22 
MotivoCancelacion22 '
{22( )
get22* -
=>22. 0 
strMotivoCancelacion221 E
;22E F
set22G J
=>22K M 
strMotivoCancelacion22N b
=22c d
value22e j
;22j k
}22l m
[44 	
System44	 
.44 
Xml44 
.44 
Serialization44 !
.44! "
XmlElementAttribute44" 5
(445 6
Form446 :
=44; <
System44= C
.44C D
Xml44D G
.44G H
Schema44H N
.44N O
XmlSchemaForm44O \
.44\ ]
Unqualified44] h
,44h i
Order44j o
=44p q
$num44r s
)44s t
]44t u
public55 
string55 
Via55 
{55 
get55 
=>55  "
strVia55# )
;55) *
set55+ .
=>55/ 1
strVia552 8
=559 :
value55; @
;55@ A
}55B C
[77 	
System77	 
.77 
Xml77 
.77 
Serialization77 !
.77! "
XmlElementAttribute77" 5
(775 6
Form776 :
=77; <
System77= C
.77C D
Xml77D G
.77G H
Schema77H N
.77N O
XmlSchemaForm77O \
.77\ ]
Unqualified77] h
,77h i
Order77j o
=77p q
$num77r s
)77s t
]77t u
public88 
string88 
OtraVia88 
{88 
get88  #
=>88$ &

strOtraVia88' 1
;881 2
set883 6
=>887 9

strOtraVia88: D
=88E F
value88G L
;88L M
}88N O
[:: 	
System::	 
.:: 
Xml:: 
.:: 
Serialization:: !
.::! "
XmlElementAttribute::" 5
(::5 6
Form::6 :
=::; <
System::= C
.::C D
Xml::D G
.::G H
Schema::H N
.::N O
XmlSchemaForm::O \
.::\ ]
Unqualified::] h
,::h i
Order::j o
=::p q
$num::r t
)::t u
]::u v
public;; 
string;; 
Aduana;; 
{;; 
get;; "
=>;;# %
	strAduana;;& /
;;;/ 0
set;;1 4
=>;;5 7
	strAduana;;8 A
=;;B C
value;;D I
;;;I J
};;K L
[== 	
System==	 
.== 
Xml== 
.== 
Serialization== !
.==! "
XmlElementAttribute==" 5
(==5 6
Form==6 :
===; <
System=== C
.==C D
Xml==D G
.==G H
Schema==H N
.==N O
XmlSchemaForm==O \
.==\ ]
Unqualified==] h
,==h i
Order==j o
===p q
$num==r t
)==t u
]==u v
public>> 
string>> 
PuertoEmbarque>> $
{>>% &
get>>' *
=>>>+ -
strPuertoEmbarque>>. ?
;>>? @
set>>A D
=>>>E G
strPuertoEmbarque>>H Y
=>>Z [
value>>\ a
;>>a b
}>>c d
[@@ 	
System@@	 
.@@ 
Xml@@ 
.@@ 
Serialization@@ !
.@@! "
XmlElementAttribute@@" 5
(@@5 6
Form@@6 :
=@@; <
System@@= C
.@@C D
Xml@@D G
.@@G H
Schema@@H N
.@@N O
XmlSchemaForm@@O \
.@@\ ]
Unqualified@@] h
,@@h i
Order@@j o
=@@p q
$num@@r t
)@@t u
]@@u v
publicAA 
stringAA 
OtroPuertoEmbarqueAA (
{AA) *
getAA+ .
=>AA/ 1!
strOtroPuertoEmbarqueAA2 G
;AAG H
setAAI L
=>AAM O!
strOtroPuertoEmbarqueAAP e
=AAf g
valueAAh m
;AAm n
}AAo p
[CC 	
SystemCC	 
.CC 
XmlCC 
.CC 
SerializationCC !
.CC! "
XmlElementAttributeCC" 5
(CC5 6
FormCC6 :
=CC; <
SystemCC= C
.CCC D
XmlCCD G
.CCG H
SchemaCCH N
.CCN O
XmlSchemaFormCCO \
.CC\ ]
UnqualifiedCC] h
,CCh i
OrderCCj o
=CCp q
$numCCr t
)CCt u
]CCu v
publicDD 
stringDD 

PaisCompraDD  
{DD! "
getDD# &
=>DD' )
strPaisCompraDD* 7
;DD7 8
setDD9 <
=>DD= ?
strPaisCompraDD@ M
=DDN O
valueDDP U
;DDU V
}DDW X
[FF 	
SystemFF	 
.FF 
XmlFF 
.FF 
SerializationFF !
.FF! "
XmlElementAttributeFF" 5
(FF5 6
FormFF6 :
=FF; <
SystemFF= C
.FFC D
XmlFFD G
.FFG H
SchemaFFH N
.FFN O
XmlSchemaFormFFO \
.FF\ ]
UnqualifiedFF] h
,FFh i
OrderFFj o
=FFp q
$numFFr t
)FFt u
]FFu v
publicGG $
ClsSolicitudesEspecialesGG '!
SolicitudesEspecialesGG( =
{HH 	
getII 
{JJ 
returnKK 
	lstSolEspKK  
;KK  !
}LL 
setMM 
{NN 
	lstSolEspOO 
=OO 
valueOO !
;OO! "
}PP 
}QQ 	
[SS 	
SystemSS	 
.SS 
XmlSS 
.SS 
SerializationSS !
.SS! "
XmlElementAttributeSS" 5
(SS5 6
FormSS6 :
=SS; <
SystemSS= C
.SSC D
XmlSSD G
.SSG H
SchemaSSH N
.SSN O
XmlSchemaFormSSO \
.SS\ ]
UnqualifiedSS] h
,SSh i
OrderSSj o
=SSp q
$numSSr t
)SSt u
]SSu v
publicTT 
ClsSubPartidasTT 
SubpartidasTT )
{UU 	
getVV 
{WW 
returnXX 

lstSubPartXX !
;XX! "
}YY 
setZZ 
{[[ 

lstSubPart\\ 
=\\ 
value\\ "
;\\" #
}]] 
}^^ 	
}__ 
}`` ﬁ'
RC:\Users\Administrador\source\repos\WSInicial\WSInicial\Capa Negocio\SubPartida.cs
	namespace 	
	WSInicial
 
. 
Capa_Negocio  
{ 
public 

class 
ClsSubPartida 
{		 
private

 
string

 
strNumeroSubpartida

 *
;

* +
private 
string $
strDescripcionSubpartida /
;/ 0
private 
string 
strUnidadMedida &
;& '
private 
ClsProductos 
lstProducto (
;( )
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
NumSubpartida #
{$ %
get& )
=>* ,
strNumeroSubpartida- @
;@ A
setB E
=>F H
strNumeroSubpartidaI \
=] ^
value_ d
;d e
}f g
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
DesSubpartida #
{$ %
get& )
=>* ,$
strDescripcionSubpartida- E
;E F
setG J
=>K M$
strDescripcionSubpartidaN f
=g h
valuei n
;n o
}p q
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
)s t
]t u
public 
string 
UnidadMedida "
{# $
get% (
=>) +
strUnidadMedida, ;
;; <
set= @
=>A C
strUnidadMedidaD S
=T U
valueV [
;[ \
}] ^
[ 	
System	 
. 
Xml 
. 
Serialization !
.! "
XmlElementAttribute" 5
(5 6
Form6 :
=; <
System= C
.C D
XmlD G
.G H
SchemaH N
.N O
XmlSchemaFormO \
.\ ]
Unqualified] h
,h i
Orderj o
=p q
$numr s
,s t
Typeu y
=z {
typeof	| Ç
(
Ç É
ClsProductos
É è
)
è ê
)
ê ë
]
ë í
public 
ClsProductos 
	Productos %
{ 	
get 
{ 
return 
lstProducto "
;" #
} 
set 
{   
lstProducto!! 
=!! 
value!! #
;!!# $
}"" 
}## 	
}$$ 
public&& 

class&& 
ClsSubPartidas&& 
{'' 
private(( 
List(( 
<(( 
ClsSubPartida(( "
>((" #

lstSubPart(($ .
;((. /
[** 	
System**	 
.** 
Xml** 
.** 
Serialization** !
.**! "
XmlElementAttribute**" 5
(**5 6
Form**6 :
=**; <
System**= C
.**C D
Xml**D G
.**G H
Schema**H N
.**N O
XmlSchemaForm**O \
.**\ ]
Unqualified**] h
,**h i
Order**j o
=**p q
$num**r s
,**s t
Type**u y
=**z {
typeof	**| Ç
(
**Ç É
ClsSubPartida
**É ê
)
**ê ë
)
**ë í
]
**í ì
public++ 
List++ 
<++ 
ClsSubPartida++ !
>++! "

Subpartida++# -
{,, 	
get-- 
{.. 
return// 

lstSubPart// !
;//! "
}00 
set11 
{22 

lstSubPart33 
=33 
value33 "
;33" #
}44 
}55 	
}66 
}77 ˝
RC:\Users\Administrador\source\repos\WSInicial\WSInicial\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str $
)$ %
]% &
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str &
)& '
]' (
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
["" 
assembly"" 	
:""	 

AssemblyVersion"" 
("" 
$str"" $
)""$ %
]""% &
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## (
)##( )
]##) *¨
GC:\Users\Administrador\source\repos\WSInicial\WSInicial\WebForm.aspx.cs
	namespace 	
	WSInicial
 
{		 
public

 

partial

 
class

 
WebForm

  
:

! "
System

# )
.

) *
Web

* -
.

- .
UI

. 0
.

0 1
Page

1 5
{ 
	protected 
void 
	Page_Load  
(  !
object! '
sender( .
,. /
	EventArgs0 9
e: ;
); <
{ 	
} 	
	protected 
void 
Button1_Click $
($ %
object% +
sender, 2
,2 3
	EventArgs4 =
e> ?
)? @
{ 	
	WSInicial 

webService  
=! "
new# &
	WSInicial' 0
(0 1
)1 2
;2 3
List 
< 
int 
> 
lstIntegers !
=" #
new$ '
List( ,
<, -
int- 0
>0 1
{2 3
$num4 5
,5 6
$num7 8
,8 9
$num: ;
}< =
;= >
Label1 
. 
Text 
= 
$str 2
+3 4

webService5 ?
.? @
Add@ C
(C D
lstIntegersD O
)O P
.P Q
ToStringQ Y
(Y Z
)Z [
;[ \
} 	
} 
} È5
IC:\Users\Administrador\source\repos\WSInicial\WSInicial\WSInicial.asmx.cs
	namespace

 	
	WSInicial


 
{ 
[ 

WebService 
( 
	Namespace 
= 
$str 1
)1 2
]2 3
[ 
WebServiceBinding 
( 

ConformsTo !
=" #
WsiProfiles$ /
./ 0
BasicProfile1_10 ?
)? @
]@ A
[ 
System 
. 
ComponentModel 
. 
ToolboxItem &
(& '
false' ,
), -
]- .
public 

class 
	WSInicial 
: 
System #
.# $
Web$ '
.' (
Services( 0
.0 1

WebService1 ;
{ 
[ 	
	WebMethod	 
] 
public 
string 

HelloWorld  
(  !
)! "
{ 	
return 
$str !
;! "
} 	
[ 	
	WebMethod	 
] 
public 
int 
Add 
( 
List 
< 
int 
>  
listInt! (
)( )
{ 	
int   
result   
=   
$num   
;   
for!! 
(!! 
int!! 
i!! 
=!! 
$num!! 
;!! 
i!! 
<!! 
listInt!!  '
.!!' (
Count!!( -
;!!- .
i!!/ 0
++!!0 2
)!!2 3
{"" 
result## 
=## 
result## 
+##  !
listInt##" )
[##) *
i##* +
]##+ ,
;##, -
}$$ 
return&& 
result&& 
;&& 
}'' 	
[JJ 	
	WebMethodJJ	 
]JJ 
publicKK 
intKK 
RecibirSolLicXMLKK #
(KK# $
ListKK$ (
<KK( )(
SolicitudLicenciaImportacionKK) E
>KKE F
lstSolLicVUCEKKG T
)KKT U
{LL 	
intMM 
	RespuestaMM 
=MM 
$numMM 
;MM 
ValidarSolicitudOO 
(OO 
lstSolLicVUCEOO *
)OO* +
;OO+ ,
returnUU 
	RespuestaUU 
;UU 
}VV 	
privateXX 
voidXX 
ValidarSolicitudXX %
(XX% &
ListXX& *
<XX* +(
SolicitudLicenciaImportacionXX+ G
>XXG H
lstSolLicVUCEXXI V
)XXV W
{YY 	
stringZZ 
strReaperturaZZ  
;ZZ  !
string[[ 
strNumTempSol[[  
;[[  !
string\\ 
strTipoSolicitud\\ #
;\\# $
string]] 
strNitImportador]] #
;]]# $
string^^ 
strNombreImportador^^ &
;^^& '
string__ 
strClaseImportador__ %
;__% &
string`` 
strTipoCancelacion`` %
;``% &
stringaa  
strMotivoCancelacionaa '
;aa' (
stringbb 
strViabb 
;bb 
stringcc 

strOtraViacc 
;cc 
stringdd 
	strAduanadd 
;dd 
stringee 
strPuertoEmbarqueee $
;ee$ %
stringff !
strOtroPuertoEmbarqueff (
;ff( )
stringgg 
strPaisCompragg  
;gg  !$
ClsSolicitudesEspecialeshh $
SolsEspshh% -
;hh- .
ClsSubPartidasii 
SubPartidasii &
;ii& '
foreachkk 
(kk (
SolicitudLicenciaImportacionkk 1
	objSolLickk2 ;
inkk< >
lstSolLicVUCEkk? L
)kkL M
{ll 
strReaperturamm 
=mm 
	objSolLicmm  )
.mm) *

Reaperturamm* 4
.mm4 5
ToStringmm5 =
(mm= >
)mm> ?
;mm? @
strNumTempSolnn 
=nn 
	objSolLicnn  )
.nn) *
NumTemporalSolnn* 8
.nn8 9
ToStringnn9 A
(nnA B
)nnB C
;nnC D
strTipoSolicitudoo  
=oo! "
	objSolLicoo# ,
.oo, -
TipoSolicitudoo- :
.oo: ;
ToStringoo; C
(ooC D
)ooD E
;ooE F
strNitImportadorpp  
=pp! "
	objSolLicpp# ,
.pp, -
NitImportadorpp- :
.pp: ;
ToStringpp; C
(ppC D
)ppD E
;ppE F
strNombreImportadorqq #
=qq$ %
	objSolLicqq& /
.qq/ 0
NomImportadorqq0 =
.qq= >
ToStringqq> F
(qqF G
)qqG H
;qqH I
strClaseImportadorrr "
=rr# $
	objSolLicrr% .
.rr. /
ClaseImportadorrr/ >
.rr> ?
ToStringrr? G
(rrG H
)rrH I
;rrI J
strTipoCancelacionss "
=ss# $
	objSolLicss% .
.ss. /
TipoCancelacionss/ >
.ss> ?
ToStringss? G
(ssG H
)ssH I
;ssI J 
strMotivoCancelaciontt $
=tt% &
	objSolLictt' 0
.tt0 1
MotivoCancelaciontt1 B
.ttB C
ToStringttC K
(ttK L
)ttL M
;ttM N
strViauu 
=uu 
	objSolLicuu "
.uu" #
Viauu# &
.uu& '
ToStringuu' /
(uu/ 0
)uu0 1
;uu1 2

strOtraViavv 
=vv 
	objSolLicvv &
.vv& '
OtraViavv' .
.vv. /
ToStringvv/ 7
(vv7 8
)vv8 9
;vv9 :
	strAduanaww 
=ww 
	objSolLicww %
.ww% &
Aduanaww& ,
.ww, -
ToStringww- 5
(ww5 6
)ww6 7
;ww7 8
strPuertoEmbarquexx !
=xx" #
	objSolLicxx$ -
.xx- .
PuertoEmbarquexx. <
.xx< =
ToStringxx= E
(xxE F
)xxF G
;xxG H!
strOtroPuertoEmbarqueyy %
=yy& '
	objSolLicyy( 1
.yy1 2
OtroPuertoEmbarqueyy2 D
.yyD E
ToStringyyE M
(yyM N
)yyN O
;yyO P
strPaisComprazz 
=zz 
	objSolLiczz  )
.zz) *

PaisComprazz* 4
.zz4 5
ToStringzz5 =
(zz= >
)zz> ?
;zz? @
SolsEsps{{ 
={{ 
	objSolLic{{ $
.{{$ %!
SolicitudesEspeciales{{% :
;{{: ;
SubPartidas|| 
=|| 
	objSolLic|| '
.||' (
Subpartidas||( 3
;||3 4
} 
}
ÄÄ 	
}
ÅÅ 
}ÇÇ 