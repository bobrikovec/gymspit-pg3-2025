//napsáno s pomocí AI, připisujeme komentáře, aby bylo vidět, že kódu rozumíme

using System;

class Program
{
    static bool AddValue(string value, string[] data, int count)
    {
        if (count >= data.Length)
        {
            Console.WriteLine("I'm afraid I can't do that.");
            return false;
        }

        data[count] = value;
        return true;        // AddValue jsme definovali jako bool, aby bylo možné zjistit, zda se přidání povedlo nebo ne, kontroluje se totiž i kapacita pole
    }

    static bool RemoveValue(string[] data, int index, int count)
    {
        if (index < 0 || index >= count)        //když na daném poli nic není, vrátí se -1 a není co odebrat, stejně tak pokud je index mimo rozsah
        {
            Console.WriteLine("I'm afraid I can't do that.");
            return false;
        }

        for (int i = index; i < count - 1; i++)
        {
            data[i] = data[i + 1];
        }
        data[count - 1] = "";
        return true;
    }

    static void AddUser(string username, string[] users, ref int userCount)
    {
        int index = Array.IndexOf(users, username);     //projde pole users, hledá hodnotu username , pokud ji najde, vrátí její index, pokud ne, vrátí -1
        if (index >= 0)
        {
            Console.WriteLine("User already exists.");
            return;
        }

        if (AddValue(username, users, userCount))       //AddValue je bool, pokud vrátí true, přidáme uživatele
        {
            userCount++;
        }
    }

    //RemoveUser jsme koncipovali tak, aby mazal uživatele i s jeho příspěvky a follow vztahy
    static void RemoveUser(string username, string[] users, ref int userCount, string[] posts, string[] postAuthors, ref int postCount, string[] followers, string[] followees, ref int followCount)
    {
        int index = Array.IndexOf(users, username);     //na stejné bázi, jen opačně
        if (index < 0)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        // nejdřív smažeme všechny příspěvky, které uživatel napsal
        for (int i = 0; i < postCount; i++)
        {
            if (postAuthors[i] == username)
            {
                RemoveValue(posts, i, postCount);
                RemoveValue(postAuthors, i, postCount);
                postCount--;
                i--; // po posunu pole zkontrolujeme stejný index znovu (cycle), posouvá se zbytek pole
            }
        }

        // potom smažeme všechny follow páry, kde je uživatel follower NEBO followee
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == username || followees[i] == username)
            {
                RemoveValue(followers, i, followCount);
                RemoveValue(followees, i, followCount);
                followCount--;
                i--; // po posunu zkontrolujeme znovu stejný index
            }
        }

        // nakonec odstraníme uživatele z pole users
        if (RemoveValue(users, index, userCount))
        {
            userCount--;
        }
    }

    // --------------------------
    // IMPLEMENTOVANÉ FUNKCE
    // --------------------------

    static void AddPost(string post, string author, string[] posts, string[] postAuthors, ref int postCount)
    {
        // uloží post a autora na stejný index
        if (AddValue(post, posts, postCount) && AddValue(author, postAuthors, postCount))
        {
            postCount++;        // Pokud se přidání obou hodnot podaří, zvýšíme počet příspěvků, obojí musí být true (AND výrok)
        }
    }

    static string[] GetUserPosts(string user, string[] posts, string[] postAuthors, int postCount)
    {
        // nejdřív zjistíme, kolik jich je
        int found = 0;
        for (int i = 0; i < postCount; i++)
        {
            if (postAuthors[i] == user)
                found++;
        }

        // vytvoříme stejně velké pole podle počtu nalezených příspěvků
        string[] result = new string[found];
        int index = 0;

        // zkopírujeme reálná data
        for (int i = 0; i < postCount; i++) //opět procházíme všechny uložené příspěvky
        {
            if (postAuthors[i] == user)
                result[index++] = posts[i];     //pokud je autor příspěvku shodný s hledaným uživatelem, uložíme příspěvek do výsledného pole a zvýšíme index
        }

        return result;      //pole result se bude zaplňovat a vrátí pole textů všech nalezených příspěvků
    }

    static void AddFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
    {
        // nesmí followovat sám sebe
        if (follower == followee)
        {
            Console.WriteLine("User cannot follow themselves.");
            return;
        }

        // hledá, jestli už takový vztah neexistuje
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == follower && followees[i] == followee)
            {
                Console.WriteLine("Already following.");
                return; //funkce je správně ukončena, pokud už takový vztah existuje
            }
        }

        // uložíme dvojici
        if (AddValue(follower, followers, followCount) && AddValue(followee, followees, followCount))
            followCount++;
    }

    static void RemoveFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
    {
        // hledá, jestli vztah existuje
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == follower && followees[i] == followee)
            {
                RemoveValue(followers, i, followCount);
                RemoveValue(followees, i, followCount);
                followCount--;
                return;
            }
        }

        Console.WriteLine("Follow pair not found.");
    }

    static string[] GetUserFollows(string user, string[] followers, string[] followees, int followCount)        //sledujeme string v poli, protože nepotřebujeme hodnotu indexu
    {
        int found = 0;

        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == user)       //tam kde je zadaný uživatel sledovatelem, zvýšíme počet nalezených uživatelů (found)
                found++;            //stejně jako u GetUserPosts, nejdřív zjistíme kolik uživatel sleduje lidí
        }

        string[] result = new string[found];       //velikost pole opět definována podle počtu nalezených uživatelů
        int index = 0;

        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == user)
                result[index++] = followees[i];     //důležité pořadí followers x followees, sdílí ale index (jeden vztah), jinak postup stejný jako u GetUserPosts
        }

        return result;
    }

    static string[] GetUserFollowers(string user, string[] followers, string[] followees, int followCount)
    {
        int found = 0;

        for (int i = 0; i < followCount; i++)
        {
            if (followees[i] == user)
                found++;
        }

        string[] result = new string[found];
        int index = 0;

        for (int i = 0; i < followCount; i++)
        {
            if (followees[i] == user)
                result[index++] = followers[i];     //opačný směr než u GetUserFollows
        }

        return result;
    }

    static string[] GetUserTimeline(string user, string[] posts, string[] postAuthors, int postCount, string[] followers, string[] followees, int followCount)
    {
        // zjistíme koho user sleduje
        string[] follows = GetUserFollows(user, followers, followees, followCount);

        // kolik příspěvků dohromady
        int found = 0;
        for (int i = 0; i < postCount; i++)
        {
            for (int j = 0; j < follows.Length; j++)
            {
                if (postAuthors[i] == follows[j])       //spojujeme indexy sledovaných daného uživatele s indexy autorů příspěvků
                    found++;        //princip stejný jako u předchozích funkcí, nejdřív zjistíme kolik příspěvků je v timeline
            }
        }

        string[] result = new string[found];
        int index = 0;

        for (int i = 0; i < postCount; i++)
        {
            for (int j = 0; j < follows.Length; j++)
            {
                if (postAuthors[i] == follows[j])
                    result[index++] = posts[i];
            }
        }

        return result;      //vypíše všechny příspěvky uživatelů, které daný uživatel sleduje (postAuthors spojuje uživatele a příspěvky v jednom vztahu)
    }

    // --------------------------
    // MAIN
    // --------------------------

    static void Main()
    {
        int MAX_USERS = 100;                //definujeme maximální počet příspěvků na uživatele
        int MAX_POSTS = MAX_USERS * 100;
        int MAX_FOLLOWS = MAX_USERS * (MAX_USERS + 1) / 2;

        string[] users = new string[MAX_USERS];     //předpřipravujeme pole pro uživatele s kapacitou MAX_USERS
        int userCount = 0;

        string[] posts = new string[MAX_POSTS];
        string[] postAuthors = new string[MAX_POSTS];
        int postCount = 0;                  //sdílí index pro obě pole, protože jsou v jednom vztahu

        string[] followers = new string[MAX_FOLLOWS];
        string[] followees = new string[MAX_FOLLOWS];
        int followCount = 0;

        // PŘÍKLADY
        AddUser("mates", users, ref userCount);        //voláme na definované funkce ze začátku a předáváme jim pole a počty, vypíšeme několik příkladů
        AddUser("ondra", users, ref userCount);         //ref (reference) protože chceme měnit hodnotu indexu v Main, konkrétně userCount
        AddUser("eva", users, ref userCount);
        AddUser("jana", users, ref userCount);
        AddUser("petr", users, ref userCount);
        AddUser("lucie", users, ref userCount);
        AddUser("karolina", users, ref userCount);

        AddPost("Hej pojďte hrát geoguessr", "mates", posts, postAuthors, ref postCount);
        AddPost("Krásné Vánoce a svátky!", "ondra", posts, postAuthors, ref postCount);
        AddPost("Vánoční haul mých dárků", "eva", posts, postAuthors, ref postCount);
        AddPost("Taky k vám nepřišel Ježíšek?", "jana", posts, postAuthors, ref postCount);
        AddPost("Takový to když musíš dělat samostatnou práci:", "petr", posts, postAuthors, ref postCount);
        AddPost("UPDATE - Vánoční haul", "eva", posts, postAuthors, ref postCount);

        AddFollow("mates", "ondra", followers, followees, ref followCount);
        AddFollow("mates", "eva", followers, followees, ref followCount);
        AddFollow("ondra", "jana", followers, followees, ref followCount);
        AddFollow("eva", "petr", followers, followees, ref followCount);
        AddFollow("jana", "lucie", followers, followees, ref followCount);
        AddFollow("petr", "karolina", followers, followees, ref followCount);
        AddFollow("mates", "jana", followers, followees, ref followCount);
        AddFollow("mates", "mates", followers, followees, ref followCount);

        RemoveFollow("ondra", "jana", followers, followees, ref followCount);
        RemoveUser("petr", users, ref userCount, posts, postAuthors, ref postCount, followers, followees, ref followCount);

        string[] matesPosts = GetUserPosts("mates", posts, postAuthors, postCount);
        Console.WriteLine("Příspěvky uživatele mates:");
        foreach (string post in matesPosts)
            Console.WriteLine(" - " + post);

        string[] evaPosts = GetUserPosts("eva", posts, postAuthors, postCount);
        Console.WriteLine("Příspěvky uživatele eva:");
        foreach (string post in evaPosts)
            Console.WriteLine(" - " + post);

        string[] matesFollows = GetUserFollows("mates", followers, followees, followCount);
        Console.WriteLine("Uživatelé sledovaní uživatelem mates:");
        foreach (string followee in matesFollows)
            Console.WriteLine(" - " + followee);

        string[] evaFollowers = GetUserFollowers("eva", followers, followees, followCount);
        Console.WriteLine("Sledující uživatele eva:");
        foreach (string follower in evaFollowers)
            Console.WriteLine(" - " + follower);

        string[] karolinaFollowers = GetUserFollowers("karolina", followers, followees, followCount);
        Console.WriteLine("Sledující uživatele karolina:");
        foreach (string follower in karolinaFollowers)
            Console.WriteLine(" - " + follower);

        string[] timeline = GetUserTimeline("mates", posts, postAuthors, postCount, followers, followees, followCount);

        Console.WriteLine("Timeline uživatele mates:");
        foreach (string post in timeline)
            Console.WriteLine(" - " + post);
    }

}

