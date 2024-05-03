import {useContext, useEffect, useRef} from 'react'
import {DroneContext} from "../SignalRContainer";

const circle = {
  width: '150px',
  height: '150px',
  borderRadius: '50%',
  border: '1px solid #1f2126',
  backgroundImage: 'linear-gradient(150deg, ' +
        'rgba(96.71694979071617, 93.3640743046999, 141.64585292339325, 1) 0%, ' +
        'rgba(46.116092428565025, 43.2159436494112, 117.27387472987175, 1) 35%)',
}

const controllerCol = {
  position: 'absolute',
  width: '46px',
  height: '130px',
  borderRadius: '10px',
  backgroundColor: '#4b4b99'
}

const controllerRow = {
  position: 'absolute',
  width: '130px',
  height: '46px',
  borderRadius: '10px',
  backgroundColor: '#4b4b99'
}

const btnUp = {
  width: '30px',
  height: '30px',
  borderRadius: '50%',
  borderColor: '#444444',
  backgroundImage: 'linear-gradient(180deg, ' +
        'rgba(96.71694979071617, 93.3640743046999, 141.64585292339325, 1) 0%, ' +
        'rgba(46.116092428565025, 43.2159436494112, 117.27387472987175, 1) 35%)',
}

const btnDown = {
  width: '30px',
  height: '30px',
  borderRadius: '50%',
  borderColor: '#444444',
  backgroundImage: 'linear-gradient(0deg, ' +
        'rgba(96.71694979071617, 93.3640743046999, 141.64585292339325, 1) 0%, ' +
        'rgba(46.116092428565025, 43.2159436494112, 117.27387472987175, 1) 35%)',
}

const btnLeft = {
  width: '30px',
  height: '30px',
  borderRadius: '50%',
  borderColor: '#444444',
  backgroundImage: 'linear-gradient(180deg, ' +
        'rgba(96.71694979071617, 93.3640743046999, 141.64585292339325, 1) 0%, ' +
        'rgba(46.116092428565025, 43.2159436494112, 117.27387472987175, 1) 35%)',
}

const btnRight = {
  width: '30px',
  height: '30px',
  borderRadius: '50%',
  borderColor: '#444444',
  backgroundImage: 'linear-gradient(180deg, ' +
        'rgba(96.71694979071617, 93.3640743046999, 141.64585292339325, 1) 0%, ' +
        'rgba(46.116092428565025, 43.2159436494112, 117.27387472987175, 1) 35%)',
}
export const DroneJoyStick = () => {
  const { handleDroneJoystick } = useContext(DroneContext)

  const intervalTime = 250  // 함수가 호출되는 간격 정의 (단위: ms)
  const intervalRef = useRef(null)  // useRef 리렌더링 이전 값에 접근할 때 사용

  // cleanup 함수 대용, When App is unmounted we should stop counter.
  useEffect(() => {
    return () => StopPressBtn()
  }, [])

  // intervalRef 를 사용하여 주기적으로 함수를 호출. 만약 이미 실행중인 경우 다시 시작되지 않도록 조건문을 사용한다.
  const StartPressBtn = (param) => {
    if (intervalRef.current) return

    // setInterval 은 일정 시간 간격으로 함수를 반복적으로 실행하는 함수이다.
    // 주어진 시간 간격마다 지정된 함수를 실행하여 반복 작업을 수행할 수 있다. (주어진 시간은 아래 [ ] 안의 시간을 기준으로 한다.)
    intervalRef.current = setInterval(() => {
      handleDroneJoystick(param)
      console.log(param)
    }, [intervalTime])
  }

  // 이전에 시작된 주기적인 동작을 중지한다. ClearInterval 함수를 사용하여 주기적으로 실행되던 작업을 중지한다.
  // intervalRef.current 를 null 로 설정하여 다시 시작되지 않도록 한다.
  const StopPressBtn = () => {
    if (intervalRef.current) {
      clearInterval(intervalRef.current)
      intervalRef.current = null
    }
  }

  return (
    <div style={circle} className={`flex h-full justify-center items-center w-full mx-auto shadow-2xl ${circle}`}>
      <div style={controllerCol} className={'flex flex-col justify-between items-center'}>
        <button style={btnUp} className='flex justify-center items-center m-1.5 pb-1'
                onMouseDown={() => StartPressBtn(0)}
                onClick={() => handleDroneJoystick(0)}
                onMouseUp={StopPressBtn}
                onMouseLeave={StopPressBtn}
        >
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style={{ transform: 'rotate(270deg)' }} className="w-5 h-5 hover:text-white">
            <path fillRule="evenodd" d="M4.5 5.653c0-1.426 1.529-2.33 2.779-1.643l11.54 6.348c1.295.712 1.295 2.573 0 3.285L7.28 19.991c-1.25.687-2.779-.217-2.779-1.643V5.653z" clipRule="evenodd" />
          </svg>
        </button>
        <button style={btnDown} className='flex justify-center items-center m-1.5 pt-1'
                onMouseDown={() => StartPressBtn(1)}
                onClick={() => handleDroneJoystick(1)}
                onMouseUp={StopPressBtn}
                onMouseLeave={StopPressBtn}
        >
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style={{ transform: 'rotate(90deg)' }} className="w-5 h-5 hover:text-white">
            <path fillRule="evenodd" d="M4.5 5.653c0-1.426 1.529-2.33 2.779-1.643l11.54 6.348c1.295.712 1.295 2.573 0 3.285L7.28 19.991c-1.25.687-2.779-.217-2.779-1.643V5.653z" clipRule="evenodd" />
          </svg>
        </button>
      </div>
      <div style={controllerRow} className={'flex flex-row justify-between items-center'}>
        <button style={btnLeft} className='flex justify-center items-center m-1.5 pr-1'
                onMouseDown={() => StartPressBtn(2)}
                onClick={() => handleDroneJoystick(2)}
                onMouseUp={StopPressBtn}
                onMouseLeave={StopPressBtn}
        >
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style={{ transform: 'rotate(180deg)' }} className="w-5 h-5 hover:text-white">
            <path fillRule="evenodd" d="M4.5 5.653c0-1.426 1.529-2.33 2.779-1.643l11.54 6.348c1.295.712 1.295 2.573 0 3.285L7.28 19.991c-1.25.687-2.779-.217-2.779-1.643V5.653z" clipRule="evenodd" />
          </svg>
        </button>
        <button style={btnRight} className='flex items-center justify-center m-1.5 pl-1'
                onMouseDown={() => StartPressBtn(3)}
                onClick={() => handleDroneJoystick(3)}
                onMouseUp={StopPressBtn}
                onMouseLeave={StopPressBtn}
        >
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-5 h-5 hover:text-white">
            <path fillRule="evenodd" d="M4.5 5.653c0-1.426 1.529-2.33 2.779-1.643l11.54 6.348c1.295.712 1.295 2.573 0 3.285L7.28 19.991c-1.25.687-2.779-.217-2.779-1.643V5.653z" clipRule="evenodd" />
          </svg>
        </button>
      </div>
    </div>
  )
}
