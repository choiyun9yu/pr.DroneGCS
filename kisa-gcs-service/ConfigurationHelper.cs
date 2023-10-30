#nullable disable // Nullable 참조 형식 기능을 비활성화하는 지시어

using Newtonsoft.Json.Linq;

namespace kisa_gcs_service
{
    public class DroneConfiguration
    {
        // Singleton 디자인 패턴
        public static DroneConfiguration Environment { get; set; }  // Environment 속성은 클래스 외부에서 Singleton 인스턴스에 엑세스할 수 있는 공용 접근 지점을 제공, get;set;은 Environmet 프로퍼티가 읽히거나 할당될 때 singleton 인스턴스를 반환하거나 설정하기 위해 사용

        private JObject configurationObj = new JObject(); // JSON 형식의 구성 정보를 저장하기 위한 객체
        private DroneConfiguration() { }    // private 생성자, Sington 클래스는 생성자가 private로 정의되어 있어 외부에서 직접 인스턴스를 생성할 수 없다.

        // 지정된 키로부터 구성 값을 가져오는 메서드
        public T GetValue<T>(string key, T def)
        {
            try
            {
                return this.configurationObj.GetValue(key).Value<T>();  // JSON 객체에서 해당 키를 찾아 T 형식으로 변환하여 반환
            }
            catch (Exception)
            {
                return def; // 예외가 발생하면 기본값(def)을 반환합니다.
            }
        }

        // 지정된 키로부터 구성 값을 가져오는 메서드 (기본값 없는 버전)
        public T GetValue<T>(string key)
        {
            return this.configurationObj.GetValue(key).Value<T>();  // JSON 객체에서 해당 키를 찾아 T 형식으로 변환하여 반환
        }

        // 구성(Configuration) 객체를 초기화하는 정적 메서드입니다.
        public static void Initialization()
        {
            DroneConfiguration.Environment = new DroneConfiguration(); // 새 구성 객체를 생성
            try
            {
                DroneConfiguration.Environment.configurationObj = JObject.Parse(File.ReadAllText(@"./config.json"));    // config.json 파일에서 JSON 데이터를 읽어 구성 객체에 저장
            }
            catch (FileNotFoundException) 
            {
                DroneConfiguration.Environment.configurationObj = new JObject();    // config.json 파일이 없을 경우 빈 JSON 객체를 사용
            }
        }
    }
}