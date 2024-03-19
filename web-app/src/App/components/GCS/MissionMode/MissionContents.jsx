import { useReducer } from 'react'

import { AltitudeChart } from '../components/AltitudeChart'
import {FlightInfoTable} from "../MiddleMap";

export const MissionContents = (props) => {

  const [isCenterBtn, toggleCenterBtn] = useReducer(
    isCenterBtn => !isCenterBtn, false
  )

  const handleCurrentCenter = () => {
    props.handleCurrentCenter()
    toggleCenterBtn()
  }

  const handleStartPointCenter = () => {
    props.handleStartPointCenter()
    toggleCenterBtn()
  }

  const handleTargetPointCenter = () => {
    props.handleTargetPointCenter()
    toggleCenterBtn()
  }

  const handleIsCenterBtn = () => {
    toggleCenterBtn()
  }

  return (
    <>
      <div className={'absolute flex flex-col justify-around items-center left-[10px] top-[200px] w-[60px] h-[25%] text-xs text-black font-normal rounded bg-white opacity-75'}>
        <div className={'flex text-lg mb-4 font-bold'}>
            Plan
        </div>

        <button
          className={'flex flex-col items-center'}
          onClick={props.toggleMissionBtn}
        >
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
            <path fillRule="evenodd"
              d="M5.625 1.5c-1.036 0-1.875.84-1.875 1.875v17.25c0 1.035.84 1.875 1.875 1.875h12.75c1.035 0 1.875-.84 1.875-1.875V12.75A3.75 3.75 0 0 0 16.5 9h-1.875a1.875 1.875 0 0 1-1.875-1.875V5.25A3.75 3.75 0 0 0 9 1.5H5.625ZM7.5 15a.75.75 0 0 1 .75-.75h7.5a.75.75 0 0 1 0 1.5h-7.5A.75.75 0 0 1 7.5 15Zm.75 2.25a.75.75 0 0 0 0 1.5H12a.75.75 0 0 0 0-1.5H8.25Z"
              clipRule="evenodd"/>
            <path
              d="M12.971 1.816A5.23 5.23 0 0 1 14.25 5.25v1.875c0 .207.168.375.375.375H16.5a5.23 5.23 0 0 1 3.434 1.279 9.768 9.768 0 0 0-6.963-6.963Z"/>
          </svg>
            Mission
        </button>

        <button
          className={'flex flex-col items-center'}
          onClick={props.toggleStationBtn}
        >
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
            <path fillRule="evenodd"
              d="M2.25 12c0-5.385 4.365-9.75 9.75-9.75s9.75 4.365 9.75 9.75-4.365 9.75-9.75 9.75S2.25 17.385 2.25 12Zm13.36-1.814a.75.75 0 1 0-1.22-.872l-3.236 4.53L9.53 12.22a.75.75 0 0 0-1.06 1.06l2.25 2.25a.75.75 0 0 0 1.14-.094l3.75-5.25Z"
              clipRule="evenodd"/>
          </svg>
            Station
        </button>

        <button className={'flex flex-col items-center'} onClick={handleIsCenterBtn}>
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
            <path
              d="M6 3a3 3 0 00-3 3v1.5a.75.75 0 001.5 0V6A1.5 1.5 0 016 4.5h1.5a.75.75 0 000-1.5H6zM16.5 3a.75.75 0 000 1.5H18A1.5 1.5 0 0119.5 6v1.5a.75.75 0 001.5 0V6a3 3 0 00-3-3h-1.5zM12 8.25a3.75 3.75 0 100 7.5 3.75 3.75 0 000-7.5zM4.5 16.5a.75.75 0 00-1.5 0V18a3 3 0 003 3h1.5a.75.75 0 000-1.5H6A1.5 1.5 0 014.5 18v-1.5zM21 16.5a.75.75 0 00-1.5 0V18a1.5 1.5 0 01-1.5 1.5h-1.5a.75.75 0 000 1.5H18a3 3 0 003-3v-1.5z"/>
          </svg>
            Center
        </button>
      </div>

      {isCenterBtn &&
            <div className={'flex p-2 flex-col items-start rounded-md text-black font-normal'} style={{
              position: 'absolute',
              top: '385px',
              left: '70px',
              opacity: '70%',
              background: '#ffff',
            }}>
              <button
                className={'px-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]'}
                onClick={handleCurrentCenter}
              >
                Drone
              </button>
              <button
                className={'px-1 my-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]'}
                onClick={handleStartPointCenter}
              >
                Start Point
              </button>
              <button
                className={'px-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]'}
                onClick={handleTargetPointCenter}
              >
                Target Point
              </button>
            </div>}

      {props.isFlightInfoTable && <FlightInfoTable/>}

      <div className={'absolute left-2 bottom-6 w-[90%] h-[200px] rounded-xl bg-black opacity-70'}>
        <AltitudeChart/>
      </div>
    </>
  )
}