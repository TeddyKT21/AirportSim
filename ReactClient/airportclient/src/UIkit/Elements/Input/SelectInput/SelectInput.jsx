import Select from "react-select"
import React from 'react';
export const SelectInput = ({options, onChange}) =>{
    return(
        <Select options={options} onChange={onChange}></Select>
    )
}