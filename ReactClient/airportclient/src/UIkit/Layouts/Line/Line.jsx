import "./Line.css"
import React from 'react';

export const Line = (props) =>{

    return (
    <div className={`Line ${props.addClass || ''}`}>{props.children}</div>
    )
}

export const Saparate = (props) => {
    return (
        <Line {...props} addClass={'Saparate'}></Line>
    )
}

export const SaparateMain = (props) => {
    return (
        <Line {...props} addClass={'Saparate Main'}></Line>
    )
}

export const Rows = (props) => {
    return (
        <Line {...props} addClass={'Rows'}></Line>
    )
}

export const RowsNavMenu = (props) =>{
    return <Line {...props} addClass={'Rows NavMenu'}></Line>
}
export const RowsScroll = (props) =>{
    return (
        <Line {...props} addClass={'RowsScroll'}></Line>
    )
}