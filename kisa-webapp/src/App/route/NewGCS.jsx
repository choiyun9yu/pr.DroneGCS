import { Outlet } from 'react-router-dom'
import { useState } from 'react'

export const NewGCS = () => {
    const [gcsMode, setGcsMode] = useState('flight')

    return <Outlet context={[gcsMode, setGcsMode]}></Outlet>
}