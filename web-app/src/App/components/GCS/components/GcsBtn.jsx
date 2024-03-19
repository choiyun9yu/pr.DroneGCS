import { LeftSideIcon } from './GcsIcon'

export const LeftSideBtn = (props) => {

  return(
    <div id='left-sidebar-btn' className={'flex justify-between m-4'}>
      {props.gcsMode === 'flight'
        ? (<button className={'m-1 p-1 rounded-md text-gray-300 bg-[#6359E9]'}>
          {LeftSideIcon.flight}
        </button>)
        : (<button className={'m-1 p-1 rounded-md text-white bg-[#27264E] hover:bg-[#6359E9]'}
          onClick={props.handleFlightMode}>
          {LeftSideIcon.flight}
        </button>)
      }
      {props.gcsMode === 'mission'
        ? (<button className={'m-1 p-1 rounded-md text-gray-300 bg-[#6359E9]'}>
          {LeftSideIcon.mission}
        </button>)
        : (<button className={'m-1 p-1 rounded-md text-white bg-[#27264E] hover:bg-[#6359E9]'}
          onClick={props.handleMissionMode}>
          {LeftSideIcon.mission}
        </button>)
      }
      {props.gcsMode === 'video'
        ? (<button className={'m-1 p-1 rounded-md text-gray-300 bg-[#6359E9]'}>
          {LeftSideIcon.video}
        </button>)
        : (<button className={'m-1 p-1 rounded-md text-white bg-[#27264E] hover:bg-[#6359E9]'}
          onClick={props.handleVideoMode}>
          {LeftSideIcon.video}
        </button>)
      }
    </div>
  )

}