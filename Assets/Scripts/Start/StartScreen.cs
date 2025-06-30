using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    private const int LEVEL_COUNT = 56;
    public GameObject loadingPanel;
    public GameObject parametersPanel;
    public GameObject aboutPanel;
    
    void Start()
    {
        ParameterReader.Instance.LockOrientation(false);

        using (var connection = new SqliteConnection(GetDatabasePath()))
        {
            connection.Open();
            int levelCount = 0;
            using (var command = connection.CreateCommand())
            {
                #if UNITY_EDITOR
                    command.CommandText = "DROP TABLE IF EXISTS level;";
                    command.ExecuteNonQuery();
                #endif

                command.CommandText = "CREATE TABLE IF NOT EXISTS level (id INTEGER PRIMARY KEY, world INTEGER, row_count INTEGER, column_count INTEGER, topDots VARCHAR(30), bottomDots VARCHAR(30), unlocked INTEGER, moveOneStar INTEGER, moveTwoStars INTEGER, moveThreeStars INTEGER, tutorial INTEGER DEFAULT 0, mode INTEGER DEFAULT 0);";
                command.ExecuteNonQuery();

                command.CommandText = "SELECT Count(id) FROM level";
 
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        levelCount = reader.GetInt32(0);
                }

                if(levelCount < LEVEL_COUNT){
                    using(IDbTransaction transaction = connection.BeginTransaction())
                    {
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (1, 1, 2, 2, \"1225\", \"2512\", 0, 4, 3, 2, 1, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (2, 1, 3, 3, \"522212223\", \"223212522\", -1, 8, 7, 6, 2, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (3, 1, 3, 3, \"122223522\", \"523222122\", -1, 8, 7, 6, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (4, 1, 3, 4, \"232522221220\", \"132222222225\", -1, 7, 6, 5, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (5, 1, 4, 3, \"125220222222\", \"322222222105\", -1, 8, 7, 6, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (6, 1, 3, 3,  \"222225132\",  \"122225322\", -1, 7, 6, 5, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (7, 1, 4, 4,  \"1225222222233202\", \"2223222222221225\", -1, 11, 10, 9, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (8, 1, 4, 4, \"2322222222251232\", \"1222222522222232\", -1, 10, 9, 8, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (9, 1, 4, 4, \"2222322122225223\", \"5222222222012322\", -1, 11, 10, 9, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (10, 1, 4, 4, \"2522202222222312\", \"2212222222032522\", -1, 8, 7, 6, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (11, 1, 4, 4, \"1025220223222032\", \"3220222202221225\", -1, 10, 9, 8, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (12, 1, 4, 4, \"2032215223222022\", \"2232022221523220\", -1, 10, 9, 8, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (13, 1, 4, 5, \"12235202022022222232\", \"02222322222030212225\", -1, 11, 10, 9, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (14, 1, 5, 5, \"2022020322222202522212032\", \"1222025222222223222220020\", -1, 12, 11, 10, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (15, 1, 5, 5, \"2322222023522220302212222\", \"1223022222522220202222320\", -1, 13, 12, 11, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (16, 1, 5, 5, \"2222522022122222222022223\", \"2223220022122222222232225\", -1, 15, 14, 13, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (17, 2, 5, 5, \"1230222222320220223232225\", \"3220502222222220223212202\", -1, 14, 13, 12, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (18, 2, 5, 6, \"122222222320220222302322222225\", \"203225222202222223222202123222\", -1, 17, 16, 15, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (19, 2, 5, 6, \"222222322325220222122222223222\", \"222203120222022220223225022222\", -1, 16, 15, 14, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (20, 2, 4, 4, \"1322222227242226\", \"2226272422221322\", -1, 8, 7, 6, 3, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (21, 2, 4, 4, \"1223272222224226\", \"2226224227221222\", -1, 12, 11, 10, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (22, 2, 4, 5, \"12022222273222024226\", \"22226222222222712232\", -1, 14, 13, 12, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (23, 2, 5, 5, \"6322222222122722032232022\", \"2222022222142722222362222\", -1, 18, 17, 16, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (24, 2, 5, 5, \"5232122222242273202227242\", \"2722422222224272022252221\", -1, 18, 17, 16, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (25, 2, 5, 6, \"222226232222222720320222221222\", \"221242222222224720222222232226\", -1, 16, 15, 14, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (26, 2, 5, 6, \"120220222422272202202222324226\", \"222226202222272220222224142022\", -1, 22, 21, 20, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (27, 2, 5, 6, \"222222025222272202222223124272\", \"122272422222272002225222022222\", -1, 17, 16, 15, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (28, 2, 5, 6, \"422230022222222227226122222242\", \"222222226122022222272022222222\", -1, 19, 18, 17, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (29, 2, 5, 6, \"122237222222220220222222324226\", \"222226322222220722242222122222\", -1, 22, 21, 20, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (30, 2, 3, 5, \"124272222037225\", \"222260222212742\", -1, 11, 10, 9, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (31, 2, 3, 3, \"128000825\", \"825222128\", -1, 6, 5, 4, 4, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (32, 2, 5, 5, \"0802230232220222220512080\", \"1228022205220202022208223\", -1, 14, 13, 12, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (33, 3, 6, 6, \"122225203222322222200203322222220322\", \"202223232202202222322323222202132325\", -1, 21, 20, 19, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (34, 3, 5, 6, \"802225222222322030222220122028\", \"123228202222222232023022822225\", -1, 15, 14, 13, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (35, 3, 5, 6, \"022230822222326227222222122824\", \"122822222222246022820203273222\", -1, 22, 21, 20, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (36, 3, 5, 6, \"204222822222221628722202223222\", \"222272222222221628822222022224\", -1, 18, 17, 16, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (37, 3, 5, 6, \"242225227222422232227322421223\", \"221222322202224020272202322226\", -1, 20, 19, 18, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (38, 3, 6, 6, \"222822232222122242222232222726282222\", \"282023222726222222124322222222322824\", -1, 23, 22, 21, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (39, 3, 6, 6, \"722223022722221222222232322222222252\", \"722252237222422422221227022222722324\", -1, 24, 23, 22, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (40, 3, 4, 6, \"122282022226220202822720\", \"827222420206027202120282\", -1, 13, 12, 11, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (41, 3, 5, 6, \"127242622322002227427232020322\", \"242220222222204272622222122422\", -1, 21, 20, 19, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (42, 3, 6, 7, \"122020822422300222222222722027222280223225\", \"222222622222282227202232222222232421032228\", -1, 21, 20, 19, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (43, 3, 6, 7, \"223222822000023082222202250323000021022322\", \"122242222220223222602228222220202272222228\", -1, 23, 22, 21, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (44, 3, 6, 7, \"222227823222772262222222222278222231722422\", \"122242228222242220222726222222222424242228\", -1, 33, 32, 26, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (45, 3, 6, 7, \"622223222422223220222222220227222222223221\", \"222222124220222222222220222227222725223222\", -1, 27, 26, 25, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (46, 3, 3, 4, \"122222222225\", \"522222222221\", -1, 7, 6, 5, 5, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (47, 3, 4, 5, \"12222222233220220225\", \"52222222222222203021\", -1, 10, 9, 8, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (48, 3, 5, 6, \"203222322252222222222223122322\", \"222221322222222322252222222222\", -1, 15, 14, 13, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (49, 4, 6, 6, \"022222242262222222722034022222222221\", \"122232222222722222222222262022022422\", -1, 23, 22, 21, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (50, 4, 6, 6, \"222221222222272223222422222222202426\", \"622227220222222230322222222232122222\", -1, 19, 18, 17, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (51, 4, 6, 6, \"722222222282212422222030222362220028\", \"822223252202222222222212282222022223\", -1, 21, 20, 19, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (52, 4, 6, 6, \"422230222222226122222232272222222222\", \"224202702222222224321522222222222237\", -1, 21, 20, 19, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (53, 4, 6, 7, \"822227222222622222222222222221222222222228\", \"820224222222122422322002222226232422722228\", -1, 22, 21, 20, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (54, 4, 6, 7, \"042222482212222203226722222202282222222220\", \"242224222282223433432222222682212222422427\", -1, 34, 33, 32, 0, 0);";
                        command.ExecuteNonQuery();
                    
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (55, 4, 6, 7, \"622242222222222232224272222320223022222221\", \"122222322202222222222220222020322222222225\", -1, 27, 26, 25, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (56, 4, 6, 7, \"824222223220280227322220222222222621222222\", \"122222222220623222227222222222240288222222\", -1, 27, 26, 18, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (57, 4, 6, 7, \"272222282222022012222222422223222622222820\", \"722382222222502022322221222382422222220272\", -1, 25, 24, 23, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (58, 4, 6, 7, \"272222220221202222222422622222022322222220\", \"222272032222242226022202222202120243242222\", -1, 27, 26, 25, 0, 1);";
                        command.ExecuteNonQuery();

                        bool isLastUnlocked = IsLevelUnlocked(command, 56);

                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (59, 4, 6, 7, \"028222222222272252222022320272222228202221\", \"122222822232422222222403262322022222722822\", " + (isLastUnlocked ? "0" : "-1") + ", 33, 30, 27, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (60, 4, 6, 7, \"202222372022122240222022232226022020222423\", \"222222022220642322242202222221222222720204\", -1, 30, 29, 28, 0, 1);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (61, 4, 6, 7, \"072222322222222222202226223242222222224012\", \"322221222232224262222222720222222230322222\", -1, 31, 30, 29, 0, 0);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO level (id, world, row_count, column_count, topDots, bottomDots, unlocked, moveOneStar, moveTwoStars, moveThreeStars, tutorial, mode) VALUES (62, 4, 6, 7, \"222232232222280212222222072282326222222203\", \"202202782325223222222221222722222283242224\", -1, 34, 33, 32, 0, 0);";
                        command.ExecuteNonQuery();
                        
                        transaction.Commit();
                    }                    
                }

                //Tutorial
                command.CommandText = "DROP TABLE IF EXISTS tutorial;";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS tutorial (id INTEGER, starting_point INTEGER, orientation INTEGER, PRIMARY KEY (id, starting_point));";
                command.ExecuteNonQuery();

                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (1, 0, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (1, 1, 3);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (2, 4, 6);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (2, 8, 1);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (2, 5, 8);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (2, 1, 4);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (3, 0, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (3, 1, 3);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (3, 5, 3);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (3, 9, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (3, 10, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (3, 11, 3);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (4, 0, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (4, 1, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (4, 6, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (4, 7, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (5, 0, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (5, 1, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (5, 2, 2);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (5, 3, 3);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO tutorial (id, starting_point, orientation) VALUES (5, 7, 3);";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            connection.Close();
        }
    }

    public bool IsLevelUnlocked(IDbCommand command, int id){
        command.CommandText = "SELECT unlocked FROM level WHERE id = "+id;
 
        using (IDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
                return reader.GetInt32(0) > 0;
        }

        return false;
    }

    public static string GetDatabasePath()
    {
        return string.Concat("URI=file:", Application.persistentDataPath, "/Mirrors.db");
    }

    public void GoToScene(string level)
    {
        loadingPanel.SetActive(true);
        Audio.Instance.PlaySound(Sound.Button);
        SceneManager.LoadSceneAsync(level);
    }

    public void ShowParameters()
    {
        Audio.Instance.PlaySound(Sound.Button);
        parametersPanel.SetActive(true);
    }

    public void ShowAbout()
    {
        Audio.Instance.PlaySound(Sound.Button);
        aboutPanel.SetActive(true);
    }

    public void CloseAbout()
    {
        Audio.Instance.PlaySound(Sound.Button);
        aboutPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (parametersPanel.activeSelf)
            {
                Audio.Instance.PlaySound(Sound.Button);
                parametersPanel.SetActive(false);
            }
            else
            {
                Audio.Instance.PlaySound(Sound.Button);
                aboutPanel.SetActive(false);
            }
        }
    }
}
