export function OnSearch(orderBy, text, fullData, setData, dateTimeFilter) {
  // console.log(minTime, maxTime, orderBy, text);

  let textFields = Object.keys(fullData[0]).filter(
    (field) => typeof fullData[0][field] === "string"
  );
  let searchWords = text
    .toLocaleLowerCase()
    .trim()
    .split(" ");
  let filteredData = fullData.filter((dataRow) => {
    if (!dateTimeFilter(dataRow)) {
      return false;
    }
    let DataString = "";
    textFields.forEach((field) => {
      DataString += ` ${dataRow[field].toLocaleLowerCase()}`;
    });
    console.log(DataString);
    let hasSearchWords = true;
    searchWords.forEach((word) => {
      if (!DataString.includes(word)) {
        hasSearchWords = false;
      }
    });
    return hasSearchWords;
  });
  if (orderBy) {
    let sortField = orderBy.value;
    filteredData.sort((f1, f2) =>
      f1[sortField] > f2[sortField] ? 1 : f2[sortField] > f1[sortField] ? -1 : 0
    );
  }
  setData(filteredData);
  if (orderBy) {
    let sortField = orderBy.value;
    filteredData.sort((f1, f2) =>
      f1[sortField] > f2[sortField] ? 1 : f2[sortField] > f1[sortField] ? -1 : 0
    );
  }
  setData(filteredData);
}
