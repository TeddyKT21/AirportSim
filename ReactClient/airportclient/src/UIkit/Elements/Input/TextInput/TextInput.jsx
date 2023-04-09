import React from 'react';
import './TextInput.css'
export const Input = ({onChange,children}) =>{
    const handleInputChange = (event) => {
        const value = event.target.value;
        onChange(value);
      };
    return(
        <input type={"text"} onBlur = {handleInputChange}></input>
    )
}