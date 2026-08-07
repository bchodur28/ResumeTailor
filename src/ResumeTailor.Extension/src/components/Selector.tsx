type SelectorProps = {
  id: number;
  rule: string;
  priority: number;
};

const Selector = ({ id, rule, priority }: SelectorProps) => {
  return (
    <div className="field-container">
      <div className="input-container">
        <label htmlFor={`rule_${id}`}>Selector Rule:</label>
        <input id={`rule_${id}`} value={rule} />
      </div>

      <div className="input-container">
        <label htmlFor={`priority_${id}`}>priority:</label>
        <input type="number" id={`priority_${id}`} value={priority} />
      </div>
      <div className="submit-btn-container">
        <button className="submit-btn">Update Selector</button>
      </div>
    </div>
  );
};

export default Selector;
