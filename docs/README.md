# Dokumentace SkautSIS

## Moduly
| ID | Název | Popis |
| -- | ----- | ----- |
| SkautSIS.Core | SkautSIS Core | Obsahuje základní nastavení SkautSISu - výběr webové služby, aplikační klíč, číslo jednotky, které web patří apod.
| SkautSIS.Users | SkautSIS Uživatelé | Modul pro přihlašování do Orchardu pomocí uživatelského účtu SkautISu. Obsahuje příslušná nastavení + možnost automatického přidělení rolí na základě rozpoznání členství v Junákovi či na základě SkautIS rolí uživatele v jednotce, které web patří. |
Moduly stačí zkopírovat do složky modulů ve vaší instalaci Orchard CMS a poté aktivovat v administraci.

Pro zprovoznění modulu SkautSIS.Users potřebujete [autorizaci vaší aplikace správci SkautISu](http://is.skaut.cz/napoveda/programatori.1-zadost-o-registraci-vytvareneho-webu-v-testovacim-skautISu.ashx). Vaše žádost by měla obsahovat:

| Položka | Hodnota |
| ------- | ------- |
| Co povolit: | Autentizace, přístup k webovým službám ostrého SkautISu. |
| Potřebné webové služby: | UserManagement.UserDetail, UserManagement.LoginUpdateRefresh, UserManagement.RoleAll, UserManagement.UserRoleAll, OrganizationUnit.PersonDetail, OrganizationUnit.PersonDetailMembership, OrganizationUnit.MembershipAllPerson, OrganizationUnit.UnitAll, OrganizationUnit.UnitDetail |
| URL na předání tokenu: | ~/SkautSIS.Users/Account/LogOn |
| URL po odhlášení: | ~/SkautSIS.Users/Account/LogOff |

Pokud se rozhodnete moduly z nějakého důvodu exportovat pomocí příkazu {{package create}}, mějte na vědomí, že dojde k přejmenování složky _Service References_ na _Service%20References_, což způsobí nefunkčnost modulů.

## Témata
| ID | Název | Popis |
| -- | ----- | ----- |
| SkautSIS.Center | SkautSIS Středisko | Základní téma pro weby středisek a podkladové téma i pro témata webů oddílů. |
| SkautSIS.Troop | SkautSIS Oddíl (zelený) | Základní téma pro weby oddílů a podkladové téma pro další barevné varianty témat webů oddílů. Je odvozené od tématu SkautSIS.Center. |
| SkautSIS.Troop.Orange | SkautSIS Oddíl (oranžový) | Oranžová varianta tématu pro weby oddílů. Je odvozené od tématu SkautSIS.Troop |
| SkautSIS.Troop.Red | SkautSIS Oddíl (červený) | Červená varianta tématu pro weby oddílů. Je odvozené od tématu SkautSIS.Troop |
| SkautSIS.Troop.Purple | SkautSIS Oddíl (fialový) | Fialová varianta tématu pro weby oddílů. Je odvozené od tématu SkautSIS.Troop |
| SkautSIS.Troop.Blue | SkautSIS Oddíl (modrý) | Modrá varianta tématu pro weby oddílů. Je odvozené od tématu SkautSIS.Troop |
| SkautSIS.Troop.Navy | SkautSIS Oddíl (tmavě modrý) | Tmavě modrá varianta tématu pro weby oddílů. Je odvozené od tématu SkautSIS.Troop |
Témata mají mnoho závislostí na externích modulech, ale všechny najdete v Galerii Orchard.

## Zdrojové kódy
Použitá strategie větvení v systému pro správu verzí (převzato z [nvie.com](http://nvie.com/posts/a-successful-git-branching-model/)):

![](Documentation_git-branching.png)

## Doporučené zdroje

### Technologie
#### Microsoft .NET
Základy jak obecně funguje, z čeho se skládá:
[http://msdn.microsoft.com/en-us/netframework/](http://msdn.microsoft.com/en-us/netframework/)

#### Orchard CMS
Na stránkách projektu Orchard je k dispozici dokumentace obsahující množství návodů pro začínající vývojáře v tomto systému:
[http://orchardproject.net/docs/](http://orchardproject.net/docs/)
Pro hlubší pochopení je dobré si projít moduly ze základní distribuce Orchard CMS.
Řešení vzniklých problémů hledat v diskuzi na stránkách projektu na CodePlex:
[http://orchard.codeplex.com/discussions](http://orchard.codeplex.com/discussions)
nebo diskuzních stránkách StackOverflow:
[http://stackoverflow.com/questions/tagged/orchardcms](http://stackoverflow.com/questions/tagged/orchardcms)

#### ASP.NET MVC
Pochopení architektury Model-View-Controller, naučení základů práce s ASP.NET MVC Frameworkem:
[http://www.asp.net/mvc](http://www.asp.net/mvc)

### SkautIS
Na stránkách nápovědy pro programátory lze nalézt návody a informace pro využívání systému SkautIS aplikacemi třetích stran:
[http://is.skaut.cz/napoveda/programatori.MainPage.ashx](http://is.skaut.cz/napoveda/programatori.MainPage.ashx)

#### Testovací SkautIS
Pro prozkoumání možností systému SkautIS, je poskytována testovací verze systému:
[http://test-is.skaut.cz/](http://test-is.skaut.cz/)
Přístupové údaje jsou uvedeny na webu nápovědy - [http://is.skaut.cz/napoveda/Testovaci-skautIS.ashx](http://is.skaut.cz/napoveda/Testovaci-skautIS.ashx)