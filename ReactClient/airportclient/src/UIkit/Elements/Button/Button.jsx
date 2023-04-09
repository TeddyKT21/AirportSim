import React from 'react';
export const Button = ({onClick,children}) =>{
    return(
        <button onClick={onClick}>{children}</button>
    )
}