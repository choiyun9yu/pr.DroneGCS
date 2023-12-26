// namespace kisa_gcs_service.Model;
//
// public class DroneConfiguration // 싱글톤 패턴을 따르면 Environment 속성은 유일한 인스턴스에 접근하기 위한 정적 속성이다.
// {
//     public static DroneConfiguration Environment { get; set; }
//     private JObject configurationObj = new JObject();
//     private DroneConfiguration() { }
//
//     // 특정 키에 대한 값을 가져오는 메서드, T는 반환할 값의 유형, key는 가져올 값의 키, def는 값이 존재하지 않을 경우 반환할 기본 값 이다.
//     public T GetValue<T>(string key, T def)
//     {
//         try
//         {
//             return configurationObj.GetValue(key).Value<T>();
//         }
//         catch (Exception)
//         {
//             return def;
//         }
//     }
//     
//     // GetValue 오버로드: 기본값이 없는 오버로드, 키에 해당하는 값을 반환
//     public T GetValue<T>(string key)
//     {
//         return configurationObj.GetValue(key).Value<T>();
//     }
//
//     // Initialization 메서드는 굿어 객체를 초기화한다. JSON 파일을 읽어와서 'JObject'로 파싱한다. 파일이 없는 경우 빈 JObject를 생성한다. 이후 Environment에 초기화된 구성을 할당한다.
//     public static void Initialization()
//     {
//         DroneConfiguration.Environment = new DroneConfiguration();
//         try
//         {
//             DroneConfiguration.Environment.configurationObj = JObject.Parse(File.ReadAllText(@"./config.json"));
//         }
//         catch (FileNotFoundException)
//         {
//             DroneConfiguration.Environment.configurationObj = new JObject();
//         }
//     }
// }
//
